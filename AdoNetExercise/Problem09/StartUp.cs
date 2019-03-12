using AdoNetExercise;
using System;
using System.Data.SqlClient;

namespace Problem09
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                Communication.ExecNonQuery(Configuration.UseDatabase, connection);

                var cmdText = "EXEC usp_GetOlder @id";

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@id", minionId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return;
                        }

                        reader.Read();
                        var name = (string)reader[0];
                        var age = (int)reader[1];
                        Console.WriteLine($"{name} – {age} years old");
                    }
                }
            }
        }
    }
}