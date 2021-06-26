using DatabaseManager.Extensions;
using DatabaseManager.ManagerEntities;
using DatabaseManager.SystemEntities;
using DataCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DatabaseManager.Core
{
    public class Manager
    {
        Database Database { get; set; }

        List<Table> Tables { get; set; }

        DatabaseConnection Connection { get; set; }

        public Manager(Database database,
                       List<Table> tables,
                       DatabaseConnection connection)
        {
            Database = database;
            Tables = tables;
            Connection = connection;
        }

        public void Setup()
        {
            if (DbNotExists())
                CreateDatabase();
            else
                UpdateDatabase();
        }

        private bool DbNotExists()
        {
            DataCore<SysDatabases> db = new DataCore<SysDatabases>(new SqlConnection(Connection.GetServerConnection));
            string sql = @$"SELECT name Name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @DatabaseName OR name = @DatabaseName)";

            return db.ExecuteQuery(sql, new { DatabaseName = Database.Name }).Count() == 0;
        }


        private void CreateDatabase()
        {
            DataCore<object> db = new DataCore<object>(new SqlConnection(Connection.GetServerConnection));
            db.ExecuteCommand(Database.GetCreateDatabase());

            db = new DataCore<object>(new SqlConnection(Connection.GetDatabaseConnection));
            foreach (Table table in Tables)
            {
                db.ExecuteCommand(table.GetCreateTable());
            }
        }

        public void UpdateDatabase()
        {
            DataCore<object> db = new DataCore<object>(new SqlConnection(Connection.GetDatabaseConnection));

            foreach (Table memoryTable in Tables)
            {
                Table dbTable = GetDbTable(memoryTable.Name);

                if (dbTable.IsNull())
                    db.ExecuteCommand(memoryTable.GetCreateTable());
                else if (!memoryTable.Equals(dbTable))
                    db.ExecuteCommand(memoryTable.GetTableModifications(dbTable));
            }
        }

        private Table GetDbTable(string tableName)
        {
            DataCore<InformationSchemaTable> db = new DataCore<InformationSchemaTable>(new SqlConnection(Connection.GetDatabaseConnection));
            string sql = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";

            InformationSchemaTable table = db.ExecuteQuery(sql, new { TableName = tableName }).FirstOrDefault();
            if (table.IsNull())
                return null;

            return new Table()
            {
                Name = table.TABLE_NAME,
                Columns = GetDbColumns(table.TABLE_NAME).ToList()
            };
        }

        private IEnumerable<Column> GetDbColumns(string tableName)
        {
            DataCore<InformationSchemaColumn> db = new DataCore<InformationSchemaColumn>(new SqlConnection(Connection.GetDatabaseConnection));

            string sql = @"SELECT Columns.COLUMN_NAME,
			                DATA_TYPE,
			                CHARACTER_MAXIMUM_LENGTH,
			                NUMERIC_PRECISION,
			                NUMERIC_SCALE,
                            (CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END ) IS_NULLABLE,
                            (CASE WHEN CONSTRAINT_NAME IS NOT NULL THEN 1 ELSE 0 END) IS_PRIMARY_KEY
                           FROM INFORMATION_SCHEMA.COLUMNS Columns
                                LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Constraints  ON Columns.COLUMN_NAME = Constraints.COLUMN_NAME
                           WHERE columns.TABLE_NAME= @TableName";

            foreach (InformationSchemaColumn column in db.ExecuteQuery(sql, new { TableName = tableName }))
            {
                yield return new Column()
                {
                    Name = column.COLUMN_NAME,
                    Size = column.CHARACTER_MAXIMUM_LENGTH.IsNotNullOrEmpty() ? column.CHARACTER_MAXIMUM_LENGTH.ToInt() : 0,
                    Type = column.DATA_TYPE.ToUpper().GetSystemType(),
                    Precision = column.NUMERIC_PRECISION.IsNotNullOrEmpty() ? column.NUMERIC_PRECISION.ToInt() : 0,
                    Scale = column.NUMERIC_SCALE.IsNotNullOrEmpty() ? column.NUMERIC_SCALE.ToInt() : 0,
                    Nullable = column.IS_NULLABLE,
                    PrimaryKey = column.IS_PRIMARY_KEY
                };
            }
        }
    }
}