using BBRAddin.Model;
using System.Text;

namespace BBRAddin.Helpers
{
    internal class JoinHelper
    {
        public static string JoinValues(string[] values, ValueType type)
        {
            var builder = new StringBuilder();

            if (type == ValueType.Uniqueidentifier || type == ValueType.DateTime)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    if (i != values.Length - 1)
                    {
                        if (i % 2 == 0)
                        {
                            builder.Append($"'{values[i]}', ");
                        }
                        else
                        {
                            builder.AppendLine($"'{values[i]}',");
                        }
                    }
                    else
                    {
                        builder.Append($"'{values[i]}'");
                    }
                }
            }
            else if (type == ValueType.Numeric)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    if (i != values.Length - 1)
                    {
                        if ((i + 1) % 5 != 0)
                        {
                            builder.Append($"{values[i]}, ");
                        }
                        else
                        {
                            builder.AppendLine($"{values[i]},");
                        }
                    }
                    else
                    {
                        builder.Append($"{values[i]}");
                    }
                }
            }
            else
            {
                for (var i = 0; i < values.Length; i++)
                {
                    var fixedValue = values[i].Replace("'", "''");
                    if (i != values.Length - 1)
                    {
                        builder.AppendLine($"N'{fixedValue}',");
                    }
                    else
                    {
                        builder.Append($"N'{fixedValue}'");
                    }
                }
            }

            return builder.ToString();
        }
    }
}