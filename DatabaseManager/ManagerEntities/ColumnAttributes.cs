using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.ManagerEntities
{
    public class ColumnAttributes
    {
        public int Size { get; private set; }

        public int Precision { get; private set; }

        public int Scale { get; private set; }

        public bool Nullable { get; private set; } = false;

        public bool PrimaryKey { get; private set; } = false;

        public object DefaultValue { get; private set; }

        public Type Type { get; private set; }

        public ColumnAttributes String(int size, bool nullable = false, bool primaryKey = false, string? defaultValue = null)
        {
            Size = size;
            Nullable = nullable;
            PrimaryKey = primaryKey;
            DefaultValue = defaultValue;
            Type = typeof(string);
            return this;
        }

        public ColumnAttributes Int(bool nullable = false, bool primaryKey = false, int? defaultValue = null)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            DefaultValue = defaultValue;
            Type = typeof(int);
            return this;
        }

        public ColumnAttributes Decimal(int precision, int scale, bool nullable = false, bool primaryKey = false, decimal? defaultValue = null)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            Precision = precision;
            Scale = scale;
            DefaultValue = defaultValue;
            Type = typeof(decimal);
            return this;
        }

        public ColumnAttributes Guid(bool primaryKey = false, Guid? defaultValue = null)
        {
            PrimaryKey = primaryKey;
            DefaultValue = defaultValue;
            Type = typeof(Guid);
            return this;
        }

        public ColumnAttributes Bool(bool nullable = false, bool primaryKey = false, bool? defaultValue = null)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            DefaultValue = defaultValue;
            Type = typeof(bool);
            return this;
        }

        public ColumnAttributes DateTime(bool nullable = false, bool primaryKey = false, DateTime? defaultValue = null)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            DefaultValue = defaultValue;
            Type = typeof(DateTime);
            return this;
        }
    }
}
