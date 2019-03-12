using AdoNetExercise;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Problem08
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionIds = Console.ReadLine()?.Split().ToList().Select(int.Parse).ToList();

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                Communication.ExecNonQuery(Configuration.UseDatabase, connection);
                var cmdText = @" UPDATE Minions
                                    SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                  WHERE Id = @Id";

                if (minionIds == null)
                {
                    return;
                }

                foreach (var minionId in minionIds)
                {
                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        command.Parameters.AddWithValue("@Id", minionId);
                        command.ExecuteNonQuery();
                    }
                }

                cmdText = "SELECT Name, Age FROM Minions";

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var name = (string)reader[0];
                        var age = (int)reader[1];
                        Console.WriteLine($"{name} {age}");
                    }
                }
            }
        }
    }
}