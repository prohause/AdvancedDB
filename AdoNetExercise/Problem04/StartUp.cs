using AdoNetExercise;
using System;
using System.Data.SqlClient;

namespace Problem04
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionInfo = Console.ReadLine()?.Split();
            var villainInfo = Console.ReadLine()?.Split();

            if (minionInfo == null) return;
            var minionName = minionInfo[1];
            var minionAge = int.Parse(minionInfo[2]);
            var minionTown = minionInfo[3];

            if (villainInfo == null) return;
            var villainName = villainInfo[1];

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                Communication.ExecNonQuery(Configuration.UseDatabase, connection);
                const string townIdQuery = @"SELECT Id FROM Towns WHERE Name = @townName";
                int? townId = null;
                int? minionId = null;
                int? villainId = null;

                using (var command = new SqlCommand(townIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@townName", minionTown);
                    townId = (int?)command.ExecuteScalar();

                    if (townId == null)
                    {
                        Communication.AddTown(minionTown, connection);
                        Console.WriteLine($"Town {minionTown} was added to the database.");
                    }

                    townId = (int?)command.ExecuteScalar();
                }

                const string minionQuery = @"SELECT Id FROM Minions WHERE Name = @Name";

                using (var command = new SqlCommand(minionQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", minionName);
                    minionId = (int?)command.ExecuteScalar();

                    if (minionId == null)
                    {
                        Communication.AddMinion(minionName, minionAge, townId, connection);
                    }

                    minionId = (int?)command.ExecuteScalar();
                }

                const string villainQuery = @"SELECT Id FROM Villains WHERE Name = @Name";

                using (var command = new SqlCommand(villainQuery,connection))
                {
                    command.Parameters.AddWithValue("@Name", villainName);
                    villainId = (int?)command.ExecuteScalar();

                    if (villainId == null)
                    {
                        Communication.AddVillain(villainName, connection);
                        Console.WriteLine($"Villain {villainName} was added to the database.");
                    }

                    villainId = (int?)command.ExecuteScalar();
                }

                const string cmdText = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

                using (var command = new SqlCommand(cmdText,connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);
                    command.Parameters.AddWithValue("@minionId", minionId);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
        }
    }
}
