﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.SystemEntities
{
    public class InformationSchemaColumn
    {
        public string COLUMN_NAME { get; set; }

        public string DATA_TYPE { get; set; }

        public string CHARACTER_MAXIMUM_LENGTH { get; set; }
    }
}
