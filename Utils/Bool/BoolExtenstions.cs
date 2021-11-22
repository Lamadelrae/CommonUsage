using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Bool
{
    public static class BoolExtenstions
    {
        public static double ToDouble(this object obj) => Convert.ToDouble(obj);

        public static bool IsNull(this object obj) => obj == null;

        public static bool IsNotNull(this object obj) => obj != null;

        public static bool IsNotNull<T>(this T obj, out T outObj)
        {
            outObj = obj != null ? obj : default;

            return obj != null;
        }

        public static bool IsNullOrEmpty(this string inputString) => string.IsNullOrEmpty(inputString);

        public static bool IsNotNullOrEmpty(this string input) => !string.IsNullOrEmpty(input);

        public static bool IsNullOrLessThanOne(this string input) => input == null || input.Length < 1;
    }
}
