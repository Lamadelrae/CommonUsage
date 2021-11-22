using DatabaseManager.Extensions;
using DatabaseManager.ManagerEntities;
using DatabaseManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace DatabaseManager.Core
{
    internal class DdlBuilder
    {
        Database Database { get; set; }

        List<Table> MemoryTables { get; set; }

        List<Table> DbTables { get; set; }

        public DdlBuilder(List<Table> memoryTables, List<Table> dbTables)
        {
            MemoryTables = memoryTables;
            DbTables = dbTables;
        }

        public DdlBuilder(Database database, List<Table> memoryTables)
        {
            Database = database;
            MemoryTables = memoryTables;
        }

        public IEnumerable<string> GenerateCreateScript()
        {
            yield return Database.CreateDatabase();

            foreach (Table table in MemoryTables)
            {
                yield return table.CreateTable();
            }
        }

        public IEnumerable<string> GenerateUpdateScript()
        {
            foreach (Difference difference in DifferenceFinder.FindDifferences(MemoryTables, DbTables))
            {
                if (difference is TableDifference tableDifference)
                    yield return GenerateScriptForTable(tableDifference);
                else if (difference is ColumnDifference columnDifference)
                    yield return GenerateScriptForColumn(columnDifference);
            }
        }

        private string GenerateScriptForTable(TableDifference tableDifference)
        {
            Table table = GetTableAccordingToDifference(tableDifference);

            return tableDifference.Action switch
            {
                TableAction.AddTable => table.CreateTable(),
                TableAction.DropTable => table.DropTable(),
                _ => throw new NotSupportedException("Table action not found.")
            };
        }

        private string GenerateScriptForColumn(ColumnDifference columnDifference)
        {
            Table table = GetTableAccordingToDifference(columnDifference);
            Column column = GetColumnByName(table, columnDifference.Name);
            return columnDifference.Action switch
            {
                ColumnAction.AddColumn => table.AddColumn(column),
                ColumnAction.DropColumn => table.DropColumn(column),
                ColumnAction.AddPk => table.AddPk(column),
                ColumnAction.DropPk => table.DropPk(column),
                ColumnAction.ModifyColumn => table.ModifyColumn(column),
                ColumnAction.AddDefault => table.AddDefaultValue(column),
                ColumnAction.DropDefault => table.DropDefaultValue(column),
                ColumnAction.ModifyDefault => table.ModifyDefaultValue(column),
                _ => throw new NotSupportedException("Column action not found.")
            };
        }

        private Table GetTableAccordingToDifference(Difference difference)
        {
            if (difference is TableDifference tableDifference)
            {
                if (tableDifference.Action.CanGetFromDb())
                    return GetTableByName(DbTables, tableDifference.Name);
                else
                    return GetTableByName(MemoryTables, tableDifference.Name);
            }
            else if (difference is ColumnDifference columnDifference)
            {
                if (columnDifference.Action.CanGetFromDb())
                    return GetTableByName(DbTables, columnDifference.TableName);
                else
                    return GetTableByName(MemoryTables, columnDifference.TableName);
            }
            else
                throw new NotSupportedException("Difference not supported.");
        }

        private Table GetTableByName(List<Table> list, string name)
        {
            return list.Where(i => i.Name == name).FirstOrDefault();
        }

        private Column GetColumnByName(Table table, string columnName)
        {
            return table.Columns.Where(i => i.Name == columnName).FirstOrDefault();
        }
    }
}