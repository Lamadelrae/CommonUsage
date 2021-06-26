using DatabaseManager.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.ManagerEntities
{
    public class Table
    {
        public string Name { get; set; }

        public List<Column> Columns { get; set; } = new List<Column>();
    }

    public class Column
    {
        public Column() { }

        public Column(string name, Type type)
        {
            Name = name;
            Type = type;

            if (IsCharacterType)
                throw new Exception("No size set.");
        }

        public Column(string name, Type type, int size)
        {
            Name = name;
            Type = type;
            Size = size;

            if (!IsCharacterType)
                throw new Exception("This Type doesn't have a size setter.");
        }

        public Column(string name, Type type, int precision, int scale)
        {
            Name = name;
            Type = type;
            Precision = precision;
            Scale = scale;

            if (!IsDecimalType)
                throw new Exception("This Type doesn't have precision.");
        }

        public Type Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Size { get; set; } = 0;

        public int Precision { get; set; }  = 0;

        public int Scale { get; set; } = 0;

        public bool IsCharacterType
        {
            get
            {
                return Type.Equals(typeof(string)) || Type.Equals(typeof(char));
            }
        }

        public bool IsDecimalType
        {
            get
            {
                return Type.Equals(typeof(decimal)) || Type.Equals(typeof(double));
            }
        }
    }
}
