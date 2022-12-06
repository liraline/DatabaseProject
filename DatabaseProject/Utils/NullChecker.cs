using System;
using System.Data.SqlClient;

namespace DatabaseProject.Utils
{
    public static class NullChecker
    {
        public static string CheckStringField(SqlDataReader queryResponse, int currentIndex)
        {
            return queryResponse.IsDBNull(currentIndex) ? null : queryResponse.GetString(currentIndex);
        }

        public static DateTime CheckDateTimeField(SqlDataReader queryResponse, int currentIndex)
        {
            return queryResponse.IsDBNull(currentIndex) ? DateTime.MinValue : queryResponse.GetDateTime(currentIndex);
        }

        public static int CheckIntField(SqlDataReader queryResponse, int currentIndex)
        {
            return queryResponse.IsDBNull(currentIndex) ? -1 : queryResponse.GetInt32(currentIndex);
        }

        public static decimal CheckDecimalField(SqlDataReader queryResponse, int currentIndex)
        {
            return queryResponse.IsDBNull(currentIndex) ? -1 : queryResponse.GetDecimal(currentIndex);
        }
    }
}
