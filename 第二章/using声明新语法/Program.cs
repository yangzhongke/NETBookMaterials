using System.Data.SqlClient;
string connStr = "Data Source=.;Initial Catalog=demo1;Integrated Security=True";

using var conn = new SqlConnection(connStr);
conn.Open();
using var cmd = conn.CreateCommand();
cmd.CommandText = "select * from T_Articles";
using var reader = cmd.ExecuteReader();
while (reader.Read())
{
}