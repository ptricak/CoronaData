using System;
using TinyCsvParser.TypeConverter;
using System.Globalization;
using System.Linq;

namespace CoronaData.Mappers
{
    public class StringWithoutQuotesTypeConverter : ITypeConverter<string>
    {
        public Type TargetType => typeof(string);

        public bool TryConvert(string value, out string result)
        {
            result = value.Replace("\"", "");
            return true;
        }
    }

    public class StringWithoutQuotesWithDistrictTypeConverter : ITypeConverter<string>
    {
        public Type TargetType => typeof(string);

        public bool TryConvert(string value, out string result)
        {
            result = value.Replace("\"", "");
            result = $"Okres {result}";
            return true;
        }
    }

    public class SlovakStringDateToDateTypeConverter : ITypeConverter<DateTime>
    {
        public Type TargetType => typeof(DateTime);

        public bool TryConvert(string value, out DateTime result)
        {
            result = DateTime.ParseExact(value, "d.M.yyyy", CultureInfo.InvariantCulture);

            return true;
        }
    }

    public class StringToIntTypeConverter : ITypeConverter<int>
    {
        public Type TargetType => typeof(int);

        public bool TryConvert(string value, out int result)
        {
            if (string.IsNullOrWhiteSpace(value))
                result = 0;
            else
            {
                var numberDecimal = Decimal.Parse(value.RemoveWhitespace(), new NumberFormatInfo { NumberDecimalSeparator = "," });
                result = (int)Math.Round(numberDecimal);
            }

            return true;
        }
    }

    #region Helpers
    public static class MappingHelper
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
    }
    #endregion Helpers
}