using DatabaseManager.SystemEntities;
using DataCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
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

            var tableentities = GetTables().ToList();
        }

        private bool DbNotExists()
        {
            DataCore<SysDatabases> db = new DataCore<SysDatabases>(new SqlConnection(Connection.GetDatabaseConnection));
            string sql = @$"SELECT name Name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @DatabaseName OR name = @DatabaseName)";

            return db.ExecuteQuery(sql, new { DatabaseName = Database.Name }).FirstOrDefault().Name != Database.Name;
        }


        private void CreateDatabase()
        {
            DataCore<object> db = new DataCore<object>(new SqlConnection(Connection.GetServerConnection));
            db.ExecuteCommand(Database.ToString());

            db = new DataCore<object>(new SqlConnection(Connection.GetDatabaseConnection));
            foreach (Table table in Tables)
            {
                db.ExecuteCommand(table.ToString());
            }
        }


        private IEnumerable<Table> GetTables()
        {
            DataCore<Table> db = new DataCore<Table>(new SqlConnection(Connection.GetDatabaseConnection));

            string sql = @"SELECT TABLE_NAME Name FROM INFORMATION_SCHEMA.TABLES";

            foreach (Table table in db.ExecuteQuery(sql))
            {
                table.Columns = GetColumns(table.Name);

                yield return table;
            }
        }

        private List<Column> GetColumns(string tableName)
        {
            DataCore<Column> db = new DataCore<Column>(new SqlConnection(Connection.GetDatabaseConnection));

            string sql = @$"SELECT COLUMN_NAME Name, DATA_TYPE Type, CHARACTER_MAXIMUM_LENGTH Size FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= @TableName";

            return db.ExecuteQuery(sql, new { TableName = tableName });
        }
    }
}