using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBRAddin.Services
{
    public static class QueryService
    {
        public static string GetSelectQuery(string tableName, string id, int selectTop = 100)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"SELECT TOP {selectTop} * FROM {tableName}");
            sb.AppendLine("WHERE RegistreringTil IS NULL AND VirkningTil IS NULL");
            if (id != null)
            {
                if (long.TryParse(id, out long result))
                {
                    sb.AppendLine($"AND Id = {result}");
                }
                else
                {
                    sb.AppendLine($"AND Id = '{id}'");
                }
            }
            sb.AppendLine("ORDER BY RegistreringFra DESC");

            return sb.ToString();
        }
    }
}
