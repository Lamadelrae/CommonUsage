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
                if (dbTable.Columns.Where(i => i.Name == memoryColumn.Name).IsNull())
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
            DataCore<Table> db = new DataCore<Table>(new SqlConnection(Connection.GetDatabaseConnection));
            string sql = @"SELECT TABLE_NAME Name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";

            Table table = db.ExecuteQuery(sql, new { TableName = tableName }).FirstOrDefault();
            if (table.IsNotNull())
                table.Columns = GetColumns(table.Name);

            return table;
        }

        private List<Column> GetColumns(string tableName)
        {
            DataCore<Column> db = new DataCore<Column>(new SqlConnection(Connection.GetDatabaseConnection));

            string sql = @$"SELECT COLUMN_NAME Name, DATA_TYPE Type, CHARACTER_MAXIMUM_LENGTH Size FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= @TableName";

            return db.ExecuteQuery(sql, new { TableName = tableName });
        }
    }
}