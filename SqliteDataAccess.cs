using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOFSEC1_Project
{
    public class SqliteDataAccess
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static void GetUsername() 
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User_LoginModel>("SELECT * from Person", new DynamicParameters());
            }
        }

        public static List<User_ProfileModel> GetUserProfile()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User_ProfileModel>("SELECT * from user_profile", new DynamicParameters());

                return output.ToList();
            }
        }

        public static void AddUser(NewUserModel newUser)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT into user_login (username, password) values (@username, @password)", newUser);
                cnn.Execute("INSERT INTO user_profile(login_Id, firstName, lastName, programId) VALUES((SELECT login_Id FROM user_login WHERE username = @username), @firstName, @lastName, (SELECT programId FROM program WHERE programCode = @program));", newUser);
            }
        }

        public static List<string> GetUsernames()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<string>("SELECT username from user_login", new DynamicParameters());

                return output.ToList();
            }
        }

        public static List<string> GetPrograms()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<string>("SELECT programCODE FROM Program", new DynamicParameters());

                return output.ToList();              
            }
        }

    }
}
