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

            List<Table> tables = new List<Table>
            {
                new Table
                {
                    Name = "Users",
                    Columns = new List<Column>
                    {
                        new Column("Username",  typeof(string), 220),
                        new Column("Password",  typeof(string), 220),
                    }
                },
                new Table
                {
                    Name = "Client",
                    Columns = new List<Column>
                    {
                        new Column("Name",  typeof(string), 220),
                        new Column("Cpf",  typeof(string), 50),
                        new Column("BirthDate",  typeof(DateTime)),
                        new Column("IdentificationDocument",  typeof(string),  60),
                        new Column("IssuingBody",  typeof(string), 100),
                        new Column("Nacionality",  typeof(string), 120),
                        new Column("Naturality",  typeof(string), 120),
                        new Column("MotherName",  typeof(string), 120),
                        new Column("FatherName",  typeof(string), 120),
                        new Column("Profission",  typeof(string), 120),
                        new Column("CivilState",  typeof(int))
                    }
                },
                new Table
                {
                    Name = "ClientAddress",
                    Columns = new List<Column>
                    {
                        new Column("ClientId",  typeof(Guid)),
                        new Column("ZipCode",  typeof(string), 50),
                        new Column("Street",  typeof(DateTime)),
                        new Column("Number",  typeof(string), 60),
                        new Column("Complement",  typeof(string), 100),
                        new Column("County",  typeof(string),  120),
                        new Column("City",  typeof(string),120),
                        new Column("State",  typeof(string), 120),
                        new Column("IsMain",  typeof(bool))
                    }
                },
                new Table
                {
                    Name = "Contact",
                    Columns = new List<Column>
                    {
                        new Column("ClientId",  typeof(Guid)),
                        new Column("Description",  typeof(string), 50),
                        new Column("Type",  typeof(int)),
                        new Column("Contact",  typeof(string), 120),
                    }
                },
                new Table
                {
                    Name = "BankAccount",
                    Columns = new List<Column>
                    {
                        new Column("ClientId",  typeof(Guid)),
                        new Column("Type",  typeof(int)),
                        new Column("Bank",  typeof(string), 120),
                        new Column("Agency",  typeof(int)),
                        new Column("AgencyDigit",  typeof(int)),
                        new Column("Account",  typeof(int)),
                        new Column("AccountDigit",  typeof(int)),
                    }
                },
                new Table
                {
                    Name = "Operation",
                    Columns = new List<Column>
                    {
                        new Column("ClientId", typeof(Guid)),
                        new Column("BankAccountId", typeof(Guid)),
                        new Column("Date", typeof(DateTime)),
                        new Column("RequestedValue", typeof(decimal), 16, 2),
                        new Column("Quota", typeof(int)),
                        new Column("FirstExpiration", typeof(DateTime)),
                        new Column("RemunerativeFeePerMonth", typeof(decimal), 5, 2),
                        new Column("RemunerativeFeePerYear", typeof(decimal), 5, 2),
                        new Column("IofPercentage", typeof(decimal), 5, 2),
                        new Column("ContractValue", typeof(decimal), 16, 2),
                        new Column("TecPerYear", typeof(decimal), 16, 2),
                        new Column("TecPerMonth", typeof(decimal), 16, 2),
                        new Column("TotalFianancedValue", typeof(decimal), 16, 2),
                    }
                }
            };

            foreach(Table t in tables)
            {
                t.Columns.InsertRange(0, GetCommonColumns());
            }


            DatabaseConnection connection = new DatabaseConnection(initialCatalog: db.Name,
                                                                   applicationName: string.Empty,
                                                                   dataSouce: @"Localhost\SQL2019",
                                                                   userId: "sa",
                                                                   password: "pass123",
                                                                   timeout: 30);

            Manager dbManager = new Manager(db, tables, connection);
            dbManager.Setup();
        }

        private List<Column> GetCommonColumns()
        {
            return new List<Column>
            {
                new Column("Id",  typeof(Guid)),
                new Column("RegistrationDate",  typeof(DateTime)),
                new Column("IsDeleted",  typeof(bool))
            };
        }
    }
}
