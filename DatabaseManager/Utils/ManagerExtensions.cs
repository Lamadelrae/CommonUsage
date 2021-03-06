using System;

namespace DatabaseManager.Extensions
{
    internal static class ManagerExtensions
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
    }
}
