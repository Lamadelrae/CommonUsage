using DatabaseManager;
using DatabaseManager.Core;
using DatabaseManager.ManagerEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateTable()
        {
            Database db = new Database("Eloan", "Eloan_Log");

            DatabaseConnection connection = new DatabaseConnection(initialCatalog: db.Name,
                                                                   applicationName: string.Empty,
                                                                   dataSouce: @"Localhost\SQL2019",
                                                                   userId: "sa",
                                                                   password: "pass123",
                                                                   timeout: 30);

            List<Table> tables = new List<Table>
            {
                new Table("Users", new List<Column>
                {
                       new Column("Username", i => i.String(200)),
                       new Column("Password", i => i.String(200)),
                       new Column("DECIMAL_TEST", i => i.Decimal(16, 2)),
                })
            };
            InsertCommonTables(tables);

            Manager dbManager = new Manager(db, tables, connection);
            dbManager.Setup();
        }

        private void InsertCommonTables(List<Table> tables)
        {
            List<Column> commonColumns = new List<Column>
            {
                new Column("Id",  i => i.Guid(nullable: true, primaryKey: true)),
                new Column("RegistrationDate",  i => i.DateTime(nullable: true, primaryKey: false)),
                new Column("IsDeleted",  i => i.Bool())
            };

            foreach (Table table in tables)
            {
                table.Columns.InsertRange(0, commonColumns);
            }
        }
    }
}
