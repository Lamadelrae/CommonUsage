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
                new Table("Users", new List<Column>
                {
                       new Column("Username", i => i.String(200)),
                       new Column("Password", i => i.String(200)),
                }),
                new Table("Client", new List<Column>
                {
                    new Column("Name",  i => i.String(120)),
                    new Column("Cpf",  i => i.String(120)),
                    new Column("BirthDate",  i => i.DateTime()),
                    new Column("IdentificationDocument",  i => i.String(200)),
                    new Column("IssuingBody",  i => i.String(200)),
                    new Column("Nacionality",  i => i.String(200)),
                    new Column("Naturality",  i => i.String(200)),
                    new Column("MotherName",  i => i.String(200)),
                    new Column("FatherName",  i => i.String(200)),
                    new Column("Profission",  i => i.String(200)),
                    new Column("CivilState",  i => i.Int())
                }),
                new Table("ClientAddress", new List<Column>
                {
                    new Column("ClientId",  i => i.Guid()),
                    new Column("ZipCode",  i => i.String(200)),
                    new Column("Street",  i => i.String(200)),
                    new Column("Number",  i => i.String(200)),
                    new Column("Complement",  i => i.String(200)),
                    new Column("County",  i => i.String(200)),
                    new Column("City",  i => i.String(200)),
                    new Column("State",  i => i.String(200)),
                    new Column("IsMain",  i => i.Bool())
                }),
                new Table("Contact", new List<Column>
                {
                    new Column("ClientId",  i => i.Guid()),
                    new Column("Description",  i => i.String(200)),
                    new Column("Type",  i => i.Int()),
                    new Column("Contact",  i => i.String(200)),
                }),
                new Table("BankAccount", new List<Column>
                {
                    new Column("ClientId",  i => i.Guid()),
                    new Column("Type",  i => i.Int()),
                    new Column("Bank",  i => i.String(120)),
                    new Column("Agency",  i => i.Int()),
                    new Column("AgencyDigit",  i => i.Int()),
                    new Column("Account",  i => i.Int()),
                    new Column("AccountDigit",  i => i.Int()),
                }),
                new Table("Operation", new List<Column>
                {
                    new Column("ClientId", i => i.Guid()),
                    new Column("BankAccountId", i => i.Guid()),
                    new Column("Date", i => i.DateTime()),
                    new Column("RequestedValue", i => i.Decimal(16, 2)),
                    new Column("Quota", i => i.Int()),
                    new Column("FirstExpiration", i => i.DateTime()),
                    new Column("RemunerativeFeePerMonth", i => i.Decimal(5, 2)),
                    new Column("RemunerativeFeePerYear", i => i.Decimal(5, 2)),
                    new Column("IofPercentage", i => i.Decimal(5, 2)),
                    new Column("ContractValue", i => i.Decimal(16, 2)),
                    new Column("TecPerYear", i => i.Decimal(16, 2)),
                    new Column("TecPerMonth", i => i.Decimal(16, 2)),
                    new Column("TotalFianancedValue", i => i.Decimal(16, 2)),
                })
            };

            foreach (Table t in tables)
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
                new Column("Id",  i => i.Guid()),
                new Column("RegistrationDate",  i => i.DateTime()),
                new Column("IsDeleted",  i => i.Bool())
            };
        }
    }
}
