using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace DatabaseManager.ManagerEntities
{
    public class Table
    {
        public Table() { }

        public Table(string name, List<Column> columns)
        {
            Name = name;
            Columns = columns;
        }

        public string Name { get; set; }

        public List<Column> Columns { get; set; } = new List<Column>();
    }
}
