using System.Data.SqlClient;

namespace AdoNetExercise
{
    public static class Communication
    {
        public static void ExecNonQuery(string cmdText, SqlConnection connection)
        {
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void AddTown(string townName,SqlConnection connection)
        {
            const string cmdText = @"INSERT INTO Towns (Name) VALUES (@townName)";
            using (var command = new SqlCommand(cmdText,connection))
            {
                command.Parameters.AddWithValue("@townName", townName);
                command.ExecuteNonQuery();
            }
        }

        public static void AddMinion(string minionName, int minionAge, int? townId, SqlConnection connection)
        {
            const string cmdText = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";
            using (var command = new SqlCommand(cmdText,connection))
            {
                command.Parameters.AddWithValue("@nam", minionName);
                command.Parameters.AddWithValue("@age", minionAge);
                command.Parameters.AddWithValue("@townId", townId);
                command.ExecuteNonQuery();
            }
        }

        public static void AddVillain(string villainName, SqlConnection connection)
        {
            const string cmdText = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
            using (var command = new SqlCommand(cmdText,connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                command.ExecuteNonQuery();
            }
        }
    }
}
