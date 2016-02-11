using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Exilion.TradingAtomics
{
    public static class Extensions
    {

        public static bool AlmostEquals(this decimal d1, decimal d2, int precision = 8)
        {
            decimal epsilon = (decimal)Math.Pow(10.0, -precision);
            return (Math.Abs(d1 - d2) <= epsilon);
        }

        public static decimal StdDevP(this IEnumerable<decimal> values)
        {
            var lst = values.ToList();
            decimal avg = lst.Average();
            decimal stdDev = (decimal)Math.Sqrt(lst.Average(v => Math.Pow((double)(v - avg), 2)));
            return stdDev;
        }

    }
    public static class EnumExtension
    {
        /// <summary>
        /// usage: [Description("text")]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
            return (attribute == null) ? value.ToString() : attribute.Description;
        }
    }
}
