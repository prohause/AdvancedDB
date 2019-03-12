using System;
using System.Data.SqlClient;
using AdoNetExercise;

namespace Problem02
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                Communication.ExecNonQuery(Configuration.UseDatabase, connection);

                const string query = "SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  \r\n    FROM Villains AS v \r\n    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId \r\nGROUP BY v.Id, v.Name \r\n  HAVING COUNT(mv.VillainId) > 3 \r\nORDER BY COUNT(mv.VillainId)";

                using (var command = new SqlCommand(query,connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = (string) reader[0];
                            var count = (int) reader[1];
                            Console.WriteLine($"{name} - {count}");
                        }
                    }

                }
            }
        }
    }
}
