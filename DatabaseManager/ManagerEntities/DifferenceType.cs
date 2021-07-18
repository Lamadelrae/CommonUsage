using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.ManagerEntities
{
    internal enum TableAction
    {
        AddTable,
        DropTable
    }

    internal enum ColumnAction
    {
        AddColumn,
        AddPk,
        DropColumn,
        DropPk,
        ModifyColumn,
        AddDefault,
        DropDefault,
        ModifyDefault
    }
}
