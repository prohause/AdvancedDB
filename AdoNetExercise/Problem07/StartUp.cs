using AdoNetExercise;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Problem07
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                Communication.ExecNonQuery(Configuration.UseDatabase, connection);

                const string cmdText = @"SELECT Name FROM Minions";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var minionsList = new List<string>();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        minionsList.Add((string)reader[0]);
                    }

                    for (int i = 0; i < minionsList.Count / 2; i++)
                    {
                        Console.WriteLine(minionsList[i]);
                        Console.WriteLine(minionsList[minionsList.Count - 1 - i]);
                    }

                    if (minionsList.Count % 2 != 2)
                    {
                        Console.WriteLine(minionsList[minionsList.Count / 2]);
                    }
                }
            }
        }
    }
}