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
        public string Name { get; private set; }

        public TableAction Action { get; private set; }

        public TableDifference(string name, TableAction action)
        {
            Name = name;
            Action = action;
        }
    }

    internal class ColumnDifference : Difference
    {
        public string TableName { get; private set; }

        public string Name { get; private set; }

        public ColumnAction Action { get; private set; }

        public ColumnDifference(string tableName,
                                string name,
                                ColumnAction action)
        {
            TableName = tableName;
            Name = name;
            Action = action;
        }
    }
}
