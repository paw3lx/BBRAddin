using System;
using ValueType = BBRAddin.Model.ValueType;

namespace BBRAddin.Helpers
{
    internal class ValueTypeHelper
    {
        public static ValueType DetermineValuesType(string[] values, out bool header)
        {
            header = false;

            var initialType = GetValueType(values[values.Length - 1]);
            if (initialType == ValueType.Text)
                return ValueType.Text;

            if (values.Length == 1)
                return initialType;

            for (var i = values.Length - 2; i >= 1; i--)
            {
                if (!EqualsValueType(values[i], initialType))
                {
                    return ValueType.Text;
                }
            }

            var firstItemType = GetValueType(values[0]);
            if (firstItemType == ValueType.Text)
            {
                header = true;
                return initialType;
            }

            if (!EqualsValueType(values[0], initialType))
            {
                return ValueType.Text;
            }

            return initialType;
        }

        private static ValueType GetValueType(string value)
        {
            decimal decimalValue;
            if (decimal.TryParse(value, out decimalValue))
                return ValueType.Numeric;

            double doubleValue;
            if (double.TryParse(value, out doubleValue))
                return ValueType.Numeric;

            Guid guidValue;
            if (Guid.TryParse(value, out guidValue))
                return ValueType.Uniqueidentifier;

            DateTime dateTimeValue;
            if (DateTime.TryParse(value, out dateTimeValue))
                return ValueType.DateTime;

            return ValueType.Text;
        }

        private static bool EqualsValueType(string value, ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.Numeric:
                    decimal decimalValue;
                    var isDecimal = decimal.TryParse(value, out decimalValue);
                    if (isDecimal)
                        return true;

                    double doubleValue;
                    return double.TryParse(value, out doubleValue);
                case ValueType.Uniqueidentifier:
                    Guid guidValue;
                    return Guid.TryParse(value, out guidValue);

                case ValueType.DateTime:
                    DateTime dateTimeValue;
                    return DateTime.TryParse(value, out dateTimeValue);
            }

            return true;
        }
    }
}