using AdoNetExercise;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Problem03
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var id = int.Parse(Console.ReadLine());
            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                Communication.ExecNonQuery(Configuration.UseDatabase, connection);

                const string query = @"SELECT Name FROM Villains WHERE Id = @Id";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var villainName = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {id} exists in the database.");
                        return;
                    }
                     Console.WriteLine($"Villain: {villainName}");
                }

                const string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                using (var command = new SqlCommand(minionsQuery,connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        var minionList = new List<string>();
                        while (reader.Read())
                        {
                            var rowNumber = (long) reader[0];
                            var minionName = (string) reader[1];
                            var minionAge = (int) reader[2];

                            minionList.Add($"{rowNumber}. {minionName} {minionAge}");
                        }

                        if (minionList.Count==0)
                        {
                            Console.WriteLine("(no minions)");
                        }
                        else
                        {
                            minionList.ForEach(Console.WriteLine);
                        }
                    }
                }
            }
        }
    }
}
