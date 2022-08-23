using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace RAPlanner
{
    public class SqliteDataAccess
    {
        public static List<Game> LoadGames()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Game>("select * from Game", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveGame(Game game)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Game (Name, Console, Link) values (@Name, @Console, @Link)", game);
            }
        }

        public static void RemoveGame(Game game)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Query("delete from Game where Id = @Id", game);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static void UpdateCompletion(Game game)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Query("update Game set Completion = @Completion where Id = @Id", game);
            }
        }
    }
}
