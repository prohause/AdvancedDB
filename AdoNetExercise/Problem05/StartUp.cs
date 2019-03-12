using AdoNetExercise;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Problem05
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var countryName = Console.ReadLine();

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                Communication.ExecNonQuery(Configuration.UseDatabase, connection);
                var cmdText = @"UPDATE Towns
                                   SET Name = UPPER(Name)
                                 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);
                    command.ExecuteNonQuery();
                }

                cmdText = @" SELECT t.Name
                               FROM Towns as t
                               JOIN Countries AS c ON c.Id = t.CountryCode
                              WHERE c.Name = @countryName";

                var townList = new List<string>();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        townList.Add((string)reader[0]);
                    }
                }

                var count = townList.Count;

                if (count > 0)
                {
                    Console.WriteLine($"{count} town names were affected.");
                    Console.WriteLine($"[{string.Join(", ", townList)}]");
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }
    }
}