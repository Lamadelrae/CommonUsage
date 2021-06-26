using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.ManagerEntities
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
    }
}
