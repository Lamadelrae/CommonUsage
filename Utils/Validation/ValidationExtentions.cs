using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Utils.Validation
{
    public static class ValidationExtentions
    {
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
