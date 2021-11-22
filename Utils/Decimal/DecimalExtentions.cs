using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Decimal
{
    public static class DecimalExtentions
    {
        public static decimal ToDecimal(this object obj) => Convert.ToDecimal(obj);
    }
}
