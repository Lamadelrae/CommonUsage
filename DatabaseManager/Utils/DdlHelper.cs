using DatabaseManager.Extensions;
using DatabaseManager.ManagerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DatabaseManager.Utils
{
    internal static class DdlHelper
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
                    columns += CreateColumn(table, column);
                else
                    columns += $", {CreateColumn(table, column)}";
            }

            if (table.Columns.Any(i => i.PrimaryKey))
            {
                Column pkColumn = table.Columns.Where(i => i.PrimaryKey).FirstOrDefault();
                columns += $", CONSTRAINT PK_{table.Name}_{pkColumn.Name} PRIMARY KEY ({pkColumn.Name})";
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
            string sql = $"ALTER TABLE {table.Name} ";
            sql += $"ADD {CreateColumn(table, column)};";
            return sql;
        }

        public static string DropColumn(this Table table, Column column)
        {
            string sql = string.Empty;
            if (column.DefaultValue.IsNotNullOrEmpty())
                sql += DropDefaultValue(table, column);

            sql += $"ALTER TABLE {table.Name} ";
            sql += $"DROP COLUMN {column.Name};";
            return sql;
        }

        public static string AddPk(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name} ";
            sql += $"ADD CONSTRAINT PK_{table.Name}_{column.Name} PRIMARY KEY({column.Name});";
            return sql;
        }

        public static string DropPk(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name} ";
            sql += $"DROP CONSTRAINT PK_{table.Name}_{column.Name};";
            return sql;
        }

        public static string AddDefaultValue(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name} ";
            sql += $"ADD CONSTRAINT DF_{table.Name}_{column.Name} ";
            sql += $"DEFAULT '{column.DefaultValue}' FOR {column.Name};";
            return sql;
        }

        public static string DropDefaultValue(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name} ";
            sql += $"DROP CONSTRAINT DF_{table.Name}_{column.Name};";
            return sql;
        }

        public static string ModifyDefaultValue(this Table table, Column column)
        {
            string sql = DropDefaultValue(table, column);
            sql += AddDefaultValue(table, column);
            return sql;
        }

        public static string ModifyColumn(this Table table, Column column)
        {
            string sql = $"ALTER TABLE {table.Name} ";
            sql += $"ALTER COLUMN {column.Name} ";
            sql += $"{column.Type.GetDbType()}";
            if (column.Size > 0)
                sql += $"({column.Size})";
            else if (column.Precision > 0 && column.Scale > 0)
                sql += $"({column.Precision}, {column.Scale})";

            if (!column.Nullable)
                sql += " NOT NULL";

            sql += ";";
            return sql;
        }

        public static string CreateColumn(this Table table, Column column)
        {
            string col = $"{column.Name}";
            col += $" {column.Type.GetDbType()}";
            if (column.Size > 0)
                col += $"({column.Size})";
            else if (column.Precision > 0 && column.Scale > 0)
                col += $"({column.Precision}, {column.Scale})";

            if (!column.Nullable)
                col += " NOT NULL";
            if (column.DefaultValue.IsNotNullOrEmpty())
                col += $" CONSTRAINT DF_{table.Name}_{column.Name} DEFAULT '{column.DefaultValue}'";

            return col;
        }

        public static bool CanGetFromDb(this ColumnAction action)
        {
            return action == ColumnAction.DropColumn ||
                   action == ColumnAction.DropDefault ||
                   action == ColumnAction.DropPk;
        }

        public static bool CanGetFromDb(this TableAction action)
        {
            return action == TableAction.DropTable;
        }
    }
}