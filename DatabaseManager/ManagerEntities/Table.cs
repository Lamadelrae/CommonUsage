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
        public Type Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Size { get; set; } = 0;
    }
}
