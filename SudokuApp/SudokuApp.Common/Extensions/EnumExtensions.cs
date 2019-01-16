using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SudokuApp.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>The get enum description.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes != null && attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "Description Not Found";
        }

        /// <summary>
        /// Получить коллекцию по значениям
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">Коллекция значений</param>
        /// <returns></returns>
        public static List<T> GetListByRange<T>(IEnumerable<int> values)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            if(values == null || !values.Any())
            {
                return null;
            }
            var allValues = Enum.GetValues(typeof(T)).Cast<T>();
            var result = new List<T>(values.Count());

            foreach (var item in allValues)
            {
                if (values.Contains((int)(object)item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static IList<SelectListItem> GetSelectListItems<T>(this Type type)
            where T : struct, IConvertible
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return ((T[]) Enum.GetValues(typeof(T))).Select(x =>
                new SelectListItem
                {
                    Value = ((int)(object)x).ToString(),
                    Text = ((Enum)(object)x).GetEnumDescription(),

                }).ToList();
        }
    }
}
