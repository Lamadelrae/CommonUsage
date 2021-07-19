using DatabaseManager.Extensions;
using DatabaseManager.ManagerEntities;
using DatabaseManager.SystemEntities;
using DatabaseManager.Utils;
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

        public void RunProcess()
        {
            if (DbNotExists())
                CreateDatabase();
            else if (HasAnyModifications())
                UpdateDatabase();
        }

        private bool DbNotExists()
        {
            DataCore<SysDatabases> db = new DataCore<SysDatabases>(new SqlConnection(Connection.GetServerConnection));
            return db.ExecuteQuery("SELECT name Name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @DatabaseName OR name = @DatabaseName)",
                new { DatabaseName = Database.Name }).Count == 0;
        }

        private bool HasAnyModifications()
        {
            return DifferenceFinder.FindDifferences(Tables, GetDbTables().ToList()).Any();
        }

        private void CreateDatabase()
        {
            DataCore<object> db = new DataCore<object>(new SqlConnection(Connection.GetServerConnection));
            DmlBuilder dmlBuilder = new DmlBuilder(Database, Tables);

            db.ExecuteCommand(dmlBuilder.GenerateCreateScript().FirstOrDefault());
            db = new DataCore<object>(new SqlConnection(Connection.GetDatabaseConnection));
            foreach (string script in dmlBuilder.GenerateCreateScript().Skip(1))
            {
                db.ExecuteCommand(script);
            }
        }

        private void UpdateDatabase()
        {
            DataCore<object> db = new DataCore<object>(new SqlConnection(Connection.GetDatabaseConnection));
            DmlBuilder dmlBuilder = new DmlBuilder(Tables, GetDbTables().ToList());
            foreach (string script in dmlBuilder.GenerateUpdateScript())
            {
                db.ExecuteCommand(script);
            }
        }

        private IEnumerable<Table> GetDbTables()
        {
            DataCore<InformationSchemaTable> db = new DataCore<InformationSchemaTable>(new SqlConnection(Connection.GetDatabaseConnection));
            string sql = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";

            foreach (InformationSchemaTable table in db.ExecuteQuery(sql))
            {
                yield return new Table()
                {
                    Name = table.TABLE_NAME,
                    Columns = GetDbColumns(table.TABLE_NAME).ToList()
                };
            }
        }

        private IEnumerable<Column> GetDbColumns(string tableName)
        {
            DataCore<InformationSchemaColumn> db = new DataCore<InformationSchemaColumn>(new SqlConnection(Connection.GetDatabaseConnection));

            string sql = @"SELECT Columns.COLUMN_NAME,
			                       DATA_TYPE,
			                       CHARACTER_MAXIMUM_LENGTH,
			                       (CASE WHEN UPPER(DATA_TYPE) IN ('DECIMAL') THEN NUMERIC_PRECISION ELSE 0 END) NUMERIC_PRECISION,
			                       (CASE WHEN UPPER(DATA_TYPE) IN ('DECIMAL') THEN NUMERIC_SCALE ELSE 0 END) NUMERIC_SCALE,
                                   (CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END ) IS_NULLABLE,
                                   (CASE WHEN CONSTRAINT_NAME IS NOT NULL THEN 1 ELSE 0 END) IS_PRIMARY_KEY,
							       (CASE WHEN UPPER(DATA_TYPE) IN ('DECIMAL') THEN 1 ELSE 0 END) HAS_PRECISION,
							       COLUMN_DEFAULT
                           FROM INFORMATION_SCHEMA.COLUMNS Columns
                                LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Constraints 
                                ON (Columns.TABLE_NAME + '_' +  Columns.COLUMN_NAME) = (Constraints.TABLE_NAME +  '_' + Constraints.COLUMN_NAME)
                           WHERE columns.TABLE_NAME = @TableName";

            foreach (InformationSchemaColumn column in db.ExecuteQuery(sql, new { TableName = tableName }))
            {
                yield return new Column()
                {
                    Name = column.COLUMN_NAME,
                    Size = column.CHARACTER_MAXIMUM_LENGTH,
                    Precision = column.NUMERIC_PRECISION,
                    Scale = column.NUMERIC_SCALE,
                    Type = column.DATA_TYPE.ToUpper().GetSystemType(),
                    Nullable = column.IS_NULLABLE,
                    PrimaryKey = column.IS_PRIMARY_KEY,
                    DefaultValue = column.COLUMN_DEFAULT.Replace("('", string.Empty).Replace("')", string.Empty).Trim()
                };
            }
        }
    }
}