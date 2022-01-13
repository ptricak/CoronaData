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
            result = DateTime.ParseExact(value, "M/d/yyyy", CultureInfo.InvariantCulture);

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

    public class PopulationStringToIntTypeConverter : ITypeConverter<int>
    {
        public Type TargetType => typeof(int);

        public bool TryConvert(string value, out int result)
        {
            if (string.IsNullOrWhiteSpace(value))
                result = 0;
            else
            {
                result = int.Parse(value.RemoveWhitespaceAndDelimiters());
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

        public static string RemoveWhitespaceAndDelimiters(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c) && !c.Equals(',') && !c.Equals('.'))
                .ToArray());
        }
    }
    #endregion Helpers
}