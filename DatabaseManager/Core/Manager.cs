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
            DataCore<SysDatabases> db = new DataCore<SysDatabases>(new SqlConnection(Connection.GetDatabaseConnection));
            string sql = @$"SELECT name Name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @DatabaseName OR name = @DatabaseName)";

            return db.ExecuteQuery(sql, new { DatabaseName = Database.Name }).Count() == 0;
        }


        private void CreateDatabase()
        {
            DataCore<object> db = new DataCore<object>(new SqlConnection(Connection.GetServerConnection));
            db.ExecuteCommand(Database.ToDdl());

            db = new DataCore<object>(new SqlConnection(Connection.GetDatabaseConnection));
            foreach (Table table in Tables)
            {
                db.ExecuteCommand(table.ToDdl());
            }
        }

        public void UpdateDatabase()
        {
            DataCore<object> db = new DataCore<object>(new SqlConnection(Connection.GetDatabaseConnection));

            foreach (Table memoryTable in Tables)
            {
                Table dbTable = GetDbTable(memoryTable.Name);

                if (dbTable.IsNull())
                    db.ExecuteCommand(memoryTable.ToDdl());
                else
                {
                    string modifications = GetTableModifications(memoryTable, dbTable);

                    if (modifications.IsNotNullOrEmpty())
                        db.ExecuteCommand(modifications);
                }
            }
        }

        public string GetTableModifications(Table memoryTable, Table dbTable)
        {
            string script = string.Empty;
            foreach (Column memoryColumn in memoryTable.Columns)
            {
                if (dbTable.Columns.Where(i => i.Name == memoryColumn.Name).FirstOrDefault().IsNull())
                    script += $"ALTER TABLE {memoryTable.Name} ADD {memoryColumn.ToColumnString()};\n";
                else
                {
                    Column dbColumn = dbTable.Columns.Where(i => i.Name == memoryColumn.Name).FirstOrDefault();

                    if (memoryColumn.Size != dbColumn.Size || memoryColumn.Type != dbColumn.Type)
                        script += $"ALTER TABLE {memoryTable.Name} ALTER COLUMN {memoryColumn.ToColumnString()};\n";
                }
            }
            return script;
        }

        private Table GetDbTable(string tableName)
        {
            DataCore<InformationSchemaTable> db = new DataCore<InformationSchemaTable>(new SqlConnection(Connection.GetDatabaseConnection));
            string sql = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";

            InformationSchemaTable table = db.ExecuteQuery(sql, new { TableName = tableName }).FirstOrDefault();
            if (table.IsNull())
                throw new Exception("Table does not exist");

            return new Table
            {
                Name = table.TABLE_NAME,
                Columns = GetColumns(table.TABLE_NAME).ToList()
            };
        }

        private IEnumerable<Column> GetColumns(string tableName)
        {
            DataCore<InformationSchemaColumn> db = new DataCore<InformationSchemaColumn>(new SqlConnection(Connection.GetDatabaseConnection));

            string sql = @$"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= @TableName";
            foreach (InformationSchemaColumn column in db.ExecuteQuery(sql, new { TableName = tableName }))
            {
                yield return new Column
                {
                    Name = column.COLUMN_NAME,
                    Size = column.CHARACTER_MAXIMUM_LENGTH.IsNotNullOrEmpty() ? column.CHARACTER_MAXIMUM_LENGTH.ToInt() : 0,
                    Type = column.DATA_TYPE.ToUpper().GetSystemType()
                };
            }
        }
    }
}