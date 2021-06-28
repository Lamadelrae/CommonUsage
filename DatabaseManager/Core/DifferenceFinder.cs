using DatabaseManager.ManagerEntities;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace DatabaseManager.Core
{
    internal static class DifferenceFinder
    {
        public static List<Difference> FindDifferences(List<Table> memoryTables, List<Table> dbTables)
        {
            List<Difference> differences = new List<Difference>();
            differences.AddRange(FindDifferencesToAdd(memoryTables, dbTables));
            differences.AddRange(FindDifferencesToRemove(memoryTables, dbTables));
            return differences;
        }

        /// <summary>
        ///  Method to find differences to add to Database
        /// </summary>
        /// <param name="memoryTables"></param>
        /// <param name="dbTables"></param>   
        /// <returns></returns>
        private static IEnumerable<Difference> FindDifferencesToAdd(List<Table> memoryTables, List<Table> dbTables)
        {
            foreach (Table memoryTable in memoryTables)
            {
                Table dbTable = dbTables.Where(i => i.Name == memoryTable.Name).FirstOrDefault();

                if (dbTable.IsNull())
                    yield return new TableDifference() { Name = memoryTable.Name, Action = TableAction.AddTable };
                else
                {
                    foreach (Column memoryColumn in memoryTable.Columns)
                    {
                        Column dbColumn = dbTable.Columns.Where(i => i.Name == memoryColumn.Name).FirstOrDefault();

                        if (dbColumn.IsNull())
                            yield return new ColumnDifference() { TableName = memoryTable.Name, Name = memoryColumn.Name, Action = ColumnAction.AddColumn };
                        else
                        {
                            if (dbColumn.Size != memoryColumn.Size || dbColumn.Precision != memoryColumn.Precision || dbColumn.Scale != memoryColumn.Scale)
                                yield return new ColumnDifference() { TableName = memoryTable.Name, Name = memoryColumn.Name, Action = ColumnAction.ModifyColumn };
                            if (!dbColumn.PrimaryKey && memoryColumn.PrimaryKey)
                                yield return new ColumnDifference() { TableName = memoryTable.Name, Name = memoryColumn.Name, Action = ColumnAction.AddPk };
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Method to find differences to remove from Database
        /// </summary>
        /// <param name="memoryTables"></param>
        /// <param name="dbTables"></param>
        /// <returns></returns>
        private static IEnumerable<Difference> FindDifferencesToRemove(List<Table> memoryTables, List<Table> dbTables)
        {
            foreach (Table dbTable in dbTables)
            {
                Table memoryTable = memoryTables.Where(i => i.Name == dbTable.Name).FirstOrDefault();

                if (memoryTable.IsNull())
                    yield return new TableDifference() { Name = dbTable.Name, Action = TableAction.DropTable };
                else
                {
                    foreach (Column dbColumn in dbTable.Columns)
                    {
                        Column memoryColumn = memoryTable.Columns.Where(i => i.Name == dbColumn.Name).FirstOrDefault();

                        if (memoryColumn.IsNull())
                            yield return new ColumnDifference() { TableName = memoryTable.Name, Name = memoryColumn.Name, Action = ColumnAction.DropColumn };
                        else
                        {
                            if (dbColumn.PrimaryKey && !memoryColumn.PrimaryKey)
                                yield return new ColumnDifference() { TableName = memoryTable.Name, Name = memoryColumn.Name, Action = ColumnAction.DropPk };
                        }
                    }
                }
            }
        }
    }
}
