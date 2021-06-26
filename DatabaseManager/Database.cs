﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class Database
    {
        public string Name { get; private set; }

        public string LogName { get; private set; }

        public Database(string name, string logName)
        {
            Name = name;
            LogName = logName;
        }

        public string FileName
        {
            get
            {
                return $@"{AppDomain.CurrentDomain.BaseDirectory}\{Name}.mdf";
            }
        }

        public string LogFileName
        {
            get
            {
                return $@"{AppDomain.CurrentDomain.BaseDirectory}\{LogName}.ldf";
            }
        }

        public override string ToString()
        {
            return @$"CREATE DATABASE {Name} ON PRIMARY (NAME = {Name},  FILENAME = '{FileName}',  SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
                                           LOG ON (NAME = {LogName},  FILENAME = '{LogFileName}',  SIZE = 1MB,  MAXSIZE = 10MB, FILEGROWTH = 10%)";
        }
    }

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
