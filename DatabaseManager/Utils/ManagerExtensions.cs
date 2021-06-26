using DatabaseManager.ManagerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Extensions
{
    public static class ManagerExtensions
    {
        public static string GetDbType(this Type type)
        {
            if (type.Equals(typeof(int)))
                return "INT";
            else if (type.Equals(typeof(decimal)))
                return "DECIMAL";
            else if (type.Equals(typeof(DateTime)))
                return "DATETIME";
            else if (type.Equals(typeof(bool)))
                return "BIT";
            else if (type.Equals(typeof(string)))
                return "VARCHAR";
            else if (type.Equals(typeof(Guid)))
                return "UNIQUEIDENTIFIER";
            else if (type.Equals(typeof(char)))
                return "CHAR";
            else
                throw new NotSupportedException();
        }

        public static Type GetSystemType(this string type)
        {
            if (type.Equals("INT"))
                return typeof(int);
            else if (type.Equals("DECIMAL"))
                return typeof(decimal);
            else if (type.Equals("DATETIME") || type.Equals("DATE"))
                return typeof(DateTime);
            else if (type.Equals("BIT"))
                return typeof(bool);
            else if (type.Equals("VARCHAR"))
                return typeof(string);
            else if (type.Equals("UNIQUEIDENTIFIER"))
                return typeof(Guid);
            else if (type.Equals("CHAR"))
                return typeof(char);
            else
                throw new NotSupportedException();
        }

        public static string ToDdl(this Table table)
        {
            string sql = string.Empty;
            sql += $"CREATE TABLE {table.Name} (";

            string columnSql = string.Empty;
            foreach (Column column in table.Columns)
            {
                if (string.IsNullOrEmpty(columnSql))
                    columnSql += column.ToColumnString();
                else
                    columnSql += $", {column.ToColumnString()}";
            }

            sql += columnSql;
            sql += " )";

            return sql;
        }

        public static string ToColumnString(this Column column)
        {
            if (column.Type.Equals(typeof(string)) && column.Size > 0)
                return $"{column.Name} {column.Type.GetDbType()} ({column.Size})";
            else
                return $"{column.Name} {column.Type.GetDbType()}";
        }

        public static string ToDdl(this Database db)
        {
            return @$"CREATE DATABASE {db.Name} ON PRIMARY (NAME = {db.Name},  FILENAME = '{db.FileName}',  SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                     LOG ON (NAME = {db.LogName},  FILENAME = '{db.LogFileName}',  SIZE = 1MB,  MAXSIZE = 10MB, FILEGROWTH = 10%)";
        }
    }
}
