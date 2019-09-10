using System;
using System.Collections.Generic;

namespace BBRAddin.Helpers
{
    class TextHelper
    {
        public static string GetFormattedText(string text)
        {
            var allValues = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var values = RemoveNullValues(allValues);

            bool header;
            var type = ValueTypeHelper.DetermineValuesType(values, out header);

            if (!header)
            {
                return JoinHelper.JoinValues(values, type);
            }

            var valuesWithoutHeader = new string[values.Length - 1];
            Array.Copy(values, 1, valuesWithoutHeader, 0, values.Length - 1);

            return JoinHelper.JoinValues(valuesWithoutHeader, type);
        }

        private static string[] RemoveNullValues(string[] values)
        {
            var filteredValues = new List<string>();

            foreach (var value in values)
            {
                if (!value.Equals("NULL"))
                {
                    filteredValues.Add(value);
                }
            }

            return filteredValues.ToArray();
        }
    }
}
