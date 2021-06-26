using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.ManagerEntities
{
    public class Column
    {
        public string Name { get; set; }

        public int Size { get; set; }

        public int Precision { get; set; }

        public int Scale { get; set; }

        public bool Nullable { get; set; } = false;

        public bool PrimaryKey { get; set; } = false;

        public Type Type { get; set; }

        public Column() { }

        public Column(string name, Func<ColumnAttributes, ColumnAttributes> columnAttributes)
        {
            ColumnAttributes attributes = columnAttributes(new ColumnAttributes());
            Name = name;
            Size = attributes.Size;
            Precision = attributes.Precision;
            Scale = attributes.Scale;
            PrimaryKey = attributes.PrimaryKey;
            Nullable = attributes.Nullable;
            Type = attributes.Type;
        }

        public override bool Equals(object column) => this.Equals(column as Column);

        public bool Equals(Column column)
        {
            return Name == column.Name &&
                   Size == column.Size &&
                   Precision == column.Precision &&
                   Scale == column.Scale &&
                   Nullable == column.Nullable &&
                   PrimaryKey == column.PrimaryKey &&
                   Type == column.Type;
        }
    }
}
