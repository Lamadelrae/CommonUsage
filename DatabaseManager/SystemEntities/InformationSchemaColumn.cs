using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.SystemEntities
{
    internal class InformationSchemaColumn
    {
        public string COLUMN_NAME { get; set; }

        public string DATA_TYPE { get; set; }

        public int CHARACTER_MAXIMUM_LENGTH { get; set; }

        public int NUMERIC_PRECISION { get; set; }

        public int NUMERIC_SCALE { get; set; }

        public string COLUMN_DEFAULT { get; set; }

        public bool IS_NULLABLE { get; set; }

        public bool IS_PRIMARY_KEY { get; set; }
    }
}
