using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBRAddin.Services
{
    public static class QueryService
    {
        public static string GetSelectQuery(string tableName, string Id, int selectTop = 100)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"SELECT TOP {selectTop} * FROM {tableName}");
            sb.AppendLine("WHERE RegistreringTil IS NULL AND VirkningTil IS NULL");
            if (long.TryParse(Id, out long result))
            {
                sb.AppendLine($"AND Id = {Id}");
            }
            else
            {
                sb.AppendLine($"AND Id = '{Id}'");
            }

            return sb.ToString();
        }
    }
}
