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

        public Type Type { get; private set; }

        public ColumnAttributes String(int size, bool nullable, bool primaryKey)
        {
            Size = size;
            Nullable = nullable;
            PrimaryKey = primaryKey;
            Type = typeof(string);
            return this;
        }

        public ColumnAttributes String(int size)
        {
            Size = size;
            Type = typeof(string);
            return this;
        }

        public ColumnAttributes Int(bool nullable, bool primaryKey)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            Type = typeof(int);
            return this;
        }

        public ColumnAttributes Int()
        {
            Type = typeof(int);
            return this;
        }

        public ColumnAttributes Decimal(int precision, int scale, bool nullable, bool primaryKey)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            Precision = precision;
            Scale = scale;
            Type = typeof(decimal);
            return this;
        }

        public ColumnAttributes Decimal(int precision, int scale)
        {
            Precision = precision;
            Scale = scale;
            Type = typeof(decimal);
            return this;
        }

        public ColumnAttributes Guid(bool nullable, bool primaryKey)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            Type = typeof(Guid);
            return this;
        }

        public ColumnAttributes Guid()
        {
            Type = typeof(Guid);
            return this;
        }

        public ColumnAttributes Bool(bool nullable, bool primaryKey)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            Type = typeof(bool);
            return this;
        }

        public ColumnAttributes Bool()
        {
            Type = typeof(bool);
            return this;
        }

        public ColumnAttributes DateTime(bool nullable, bool primaryKey)
        {
            Nullable = nullable;
            PrimaryKey = primaryKey;
            Type = typeof(DateTime);
            return this;
        }

        public ColumnAttributes DateTime()
        {
            Type = typeof(DateTime);
            return this;
        }
    }
}
