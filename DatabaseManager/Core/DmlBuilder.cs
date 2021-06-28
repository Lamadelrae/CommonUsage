using DatabaseManager.Extensions;
using DatabaseManager.ManagerEntities;
using DatabaseManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace DatabaseManager.Core
{
    internal class DmlBuilder
    {
        Database Database { get; set; }

        List<Table> MemoryTables { get; set; }

        List<Table> DbTables { get; set; }

        public DmlBuilder(List<Table> memoryTables, List<Table> dbTables)
        {
            MemoryTables = memoryTables;
            DbTables = dbTables;
        }

        public DmlBuilder(Database database, List<Table> memoryTables)
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
                if (difference is TableDifference)
                    yield return GenerateScriptForTable((TableDifference)difference);
                else if (difference is ColumnDifference)
                    yield return GenerateScriptForColumn((ColumnDifference)difference);
            }
        }

        private string GenerateScriptForTable(TableDifference tableDifference)
        {
            if (tableDifference.Action == TableAction.AddTable)
                return GetTableByName(MemoryTables, tableDifference.Name).CreateTable();
            else if (tableDifference.Action == TableAction.DropTable)
                return GetTableByName(MemoryTables, tableDifference.Name).DropTable();
            else
                throw new NotSupportedException();
        }

        private string GenerateScriptForColumn(ColumnDifference columnDifference)
        {
            Table table = GetTableByName(MemoryTables, columnDifference.TableName);
            Column column = GetColumnByTableAndName(MemoryTables, columnDifference.TableName, columnDifference.Name);

            if (columnDifference.Action == ColumnAction.AddColumn)
                return table.AddColumn(column);
            else if (columnDifference.Action == ColumnAction.DropColumn)
                return table.DropColumn(column);
            else if (columnDifference.Action == ColumnAction.AddPk)
                return table.AddPk(column);
            else if (columnDifference.Action == ColumnAction.DropPk)
                return table.DropPk(column);
            else if (columnDifference.Action == ColumnAction.ModifyColumn)
                return table.ModifyColumn(column);
            else
                throw new NotSupportedException();
        }

        private Table GetTableByName(List<Table> list, string name)
        {
            return list.Where(i => i.Name == name).FirstOrDefault();
        }

        private Column GetColumnByTableAndName(List<Table> list, string tableName, string columnName)
        {
            return list.Where(i => i.Name == tableName).FirstOrDefault()
                    .Columns.Where(i => i.Name == columnName).FirstOrDefault();
        }
    }
}