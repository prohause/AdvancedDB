using AdoNetExercise;
using System;
using System.Data.SqlClient;

namespace Problem06
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var villainId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                Communication.ExecNonQuery(Configuration.UseDatabase, connection);

                var cmdText = @"SELECT Name FROM Villains WHERE Id = @villainId";
                string villainName = null;

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);
                    villainName = (string)command.ExecuteScalar();
                }

                if (villainName == null)
                {
                    Console.WriteLine("No such villain was found.");
                    return;
                }

                cmdText = @"DELETE FROM MinionsVillains
                             WHERE VillainId = @villainId";
                var minionCount = 0;
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);
                    minionCount = command.ExecuteNonQuery();
                }

                cmdText = @"DELETE FROM Villains
                             WHERE Id = @villainId";

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{minionCount} minions were released.");
            }
        }
    }
}