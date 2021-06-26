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

        public override bool Equals(object dbTable) => this.Equals(dbTable as Table);

        public bool Equals(Table dbTable)
        {
            foreach (Column column in Columns)
            {
                Column dbColumn = dbTable.Columns.Where(i => i.Name == column.Name).FirstOrDefault();

                if (dbColumn.IsNull())
                    return false;
                else if (!column.Equals(dbColumn))
                    return false;
            }

            return true;
        }
    }
}
