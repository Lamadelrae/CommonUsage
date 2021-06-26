using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Extensions
    {
        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static bool IsNotNull<T>(this T obj, out T outObj)
        {
            outObj = obj != null ? obj : default;

            return obj != null;
        }

        public static bool IsNullOrEmpty(this string inputString)
        {
            return string.IsNullOrEmpty(inputString);
        }

        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        public static bool IsNullOrLessThanOne(this string input)
        {
            return input == null || input.Length < 1;
        }

        public static string ToBrl(this decimal input) => input.ToString("c", CultureInfo.GetCultureInfo("pt-BR")).Replace("R$", string.Empty).Trim();

        public static string ToBrlWithSymbol(this decimal input) => input.ToString("c", CultureInfo.GetCultureInfo("pt-BR"));

        public static DateTime ToDateTime(this object obj) => Convert.ToDateTime(obj);

        public static string ToSha256(this string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();

                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }

        public static K Map<T, K>(this T obj, Func<T, K> func) => func(obj);

        public static List<K> MapToList<T, K>(this List<T> objList, Func<T, K> func)
        {
            List<K> outputObjList = new List<K>();

            objList.ForEach(obj =>
            {
                outputObjList.Add(func(obj));
            });

            return outputObjList;
        }

        public static void IsValid<T>(this T obj)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);

            if (!isValid)
            {
                string errors = string.Empty;

                foreach (var error in results)
                    errors += $"{error}\n";

                throw new Exception(errors);
            }
        }
    }
}
