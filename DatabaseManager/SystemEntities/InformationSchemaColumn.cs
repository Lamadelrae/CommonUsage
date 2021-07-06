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

        public string CHARACTER_MAXIMUM_LENGTH { get; set; }

        public string NUMERIC_PRECISION { get; set; }

        public string NUMERIC_SCALE { get; set; }

        public string COLUMN_DEFAULT { get; set; }

        public bool IS_NULLABLE { get; set; }

        public bool IS_PRIMARY_KEY { get; set; }

        public bool HAS_PRECISION { get; set; }
    }
}
