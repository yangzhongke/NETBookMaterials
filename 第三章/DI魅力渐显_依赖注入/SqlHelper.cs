using System.Data;

namespace DI魅力渐显_依赖注入
{
    static class SqlHelper
    {
        public static DataTable ExecuteQuery(this IDbConnection conn, FormattableString formattable)
        {
            using IDbCommand cmd = CreateCommand(conn, formattable);
            DataTable dt = new DataTable();
            using var reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        public static object? ExecuteScalar(this IDbConnection conn, FormattableString formattable)
        {
            using IDbCommand cmd = CreateCommand(conn, formattable);
            return cmd.ExecuteScalar();
        }

        public static int ExecuteNonQuery(this IDbConnection conn, FormattableString formattable)
        {
            using IDbCommand cmd = CreateCommand(conn, formattable);
            int result = cmd.ExecuteNonQuery();
            return result;
        }

        private static IDbCommand CreateCommand(IDbConnection conn, FormattableString formattable)
        {
            var cmd = conn.CreateCommand();
            string sql = formattable.Format;
            for (int i = 0; i < formattable.ArgumentCount; i++)
            {
                sql = sql.Replace("{" + i + "}", "@p" + i);
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = "@p" + i;
                parameter.Value = formattable.GetArgument(i);
                cmd.Parameters.Add(parameter);
            }
            cmd.CommandText = sql;
            return cmd;
        }
    }
}
