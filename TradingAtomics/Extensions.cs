using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TradingAtomics
{
    public static class Extensions
    {
        public static string FormatAs(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

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

        public static T DeepClone<T>(this T a)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
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
