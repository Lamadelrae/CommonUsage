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
            Database db = new Database("TESTING_DB", "TESTING_DB_LOG");

            List<Table> tables = new List<Table>
            {
                new Table
                {
                   Name = "FUCK_TABLES_MAN",
                   Columns = new List<Column>
                   {
                       new Column { Name = "TEST_VARCHAR", Size = 120, Type = typeof(string)},
                       new Column { Name = "TEST_INT", Type = typeof(int)},
                       new Column { Name = "TEST_DATE", Type = typeof(DateTime)},
                   }
                }
            };

             DatabaseConnection connection = new DatabaseConnection(initialCatalog: "TESTING_DB",
                                                                    applicationName: string.Empty,
                                                                    dataSouce: @"Localhost\SQL2019",
                                                                    userId: "sa",
                                                                    password: "pass123",
                                                                    timeout: 30);
            
            Manager dbManager = new Manager(db, tables, connection);
            dbManager.Setup();
        }
    }
}
