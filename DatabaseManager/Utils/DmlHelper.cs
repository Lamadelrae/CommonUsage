using DatabaseManager.Extensions;
using DatabaseManager.ManagerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Utils
{
    internal static class DmlHelper
    {
        public static string CreateDatabase(this Database database)
        {
            return @$"CREATE DATABASE {database.Name} ON PRIMARY (NAME = {database.Name},  FILENAME = '{database.FileName}',  SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                     LOG ON (NAME = {database.LogName},  FILENAME = '{database.LogFileName}',  SIZE = 1MB,  MAXSIZE = 10MB, FILEGROWTH = 10%);";
        }

        public static string CreateTable(this Table table)
        {
            string sql = string.Empty;
            sql += $"CREATE TABLE {table.Name} ( ";

            string columns = string.Empty;
            foreach (Column column in table.Columns)
            {
                if (string.IsNullOrEmpty(columns))
                    columns += CreateColumn(column);
                else
                    columns += $", {CreateColumn(column)}";
            }

            sql += columns;
            sql += " );";

            return sql;
        }

        public static string DropTable(this Table table)
        {
            return $"DROP TABLE {table.Name};";
        }

        public static string AddColumn(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name}";
            sql += $"ADD {CreateColumn(column)};";
            return sql;
        }

        public static string DropColumn(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name}";
            sql += $"DROP COLUMN {column.Name};";
            return sql;

        }

        public static string AddPk(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name}";
            sql += $"ADD CONSTRAINT PK_{table.Name}_{column.Name} PRIMARY KEY({column.Name});";
            return sql;
        }

        public static string DropPk(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name}";
            sql += $"DROP CONSTRAINT PK_{table.Name}_{column.Name};";
            return sql;
        }

        public static string ModifyColumn(this Table table, Column column)
        {
            string sql = $"ALER TABLE {table.Name}";
            sql += $"ALTER COLUMN {column.Name}";
            sql += $" {column.Type.GetDbType()}";
            if (column.Size > 0)
                sql += $"({column.Size})";
            else if (column.Precision > 0 && column.Scale > 0)
                sql += $"({column.Precision}, {column.Scale})";

            if (!column.Nullable)
                sql += " NOT NULL";

            sql += ";";
            return sql;
        }

        public static string CreateColumn(this Column column)
        {
            string col = $"{column.Name}";
            col += $" {column.Type.GetDbType()}";
            if (column.Size > 0)
                col += $"({column.Size})";
            else if (column.Precision > 0 && column.Scale > 0)
                col += $"({column.Precision}, {column.Scale})";

            if (column.PrimaryKey)
                col += " PRIMARY KEY";
            if (!column.Nullable)
                col += " NOT NULL";
            return col;
        }
    }
}
