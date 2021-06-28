using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.ManagerEntities
{
    internal class Difference { }

    internal class TableDifference : Difference
    {
        public string Name { get; set; }

        public TableAction Action { get; set; }
    }

    internal class ColumnDifference : Difference
    {
        public string TableName { get; set; }

        public string Name { get; set; }

        public ColumnAction Action { get; set; }
    }
}
