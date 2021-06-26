using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.ManagerEntities
{
    public class DatabaseConnection
    {
        public string InitialCatalog { get; private set; }

        public string ApplicationName { get; private set; }

        public string DataSource { get; private set; }

        public string UserId { get; private set; }

        public string Password { get; private set; }

        public int Timeout { get; private set; }

        public DatabaseConnection(string initialCatalog,
                                  string applicationName,
                                  string dataSouce,
                                  string userId,
                                  string password,
                                  int timeout)
        {
            InitialCatalog = initialCatalog;
            ApplicationName = applicationName;
            DataSource = dataSouce;
            UserId = userId;
            Password = password;
            Timeout = timeout;
        }

        public string GetServerConnection
        {
            get
            {
                return new SqlConnectionStringBuilder
                {
                    DataSource = DataSource,
                    UserID = UserId,
                    Password = Password,
                    MultipleActiveResultSets = true,
                    ConnectTimeout = Timeout
                }.ToString();
            }
        }

        public string GetDatabaseConnection
        {
            get
            {
                return new SqlConnectionStringBuilder
                {
                    DataSource = DataSource,
                    UserID = UserId,
                    Password = Password,
                    MultipleActiveResultSets = true,
                    ConnectTimeout = Timeout,
                    InitialCatalog = InitialCatalog,
                    ApplicationName = ApplicationName
                }.ToString();
            }
        }
    }
}
