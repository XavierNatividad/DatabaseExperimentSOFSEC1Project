using Dapper;
using GradeCalculator;
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

        public static void AddUser(NewUserModel newUser, string password)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT into user_login (username, password) values (@username, @password)", newUser);
                cnn.Execute("INSERT INTO user_profile(login_Id, firstName, lastName, programId) VALUES((SELECT login_Id FROM user_login WHERE username = @username), @firstName, @lastName, @program);", newUser);
            }
        }

        public static void AddUser(NewUserModel newUser, string password, string programCode)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT into user_login (username, password) values (@username, @password)", newUser);
                cnn.Execute("INSERT INTO user_profile(login_Id, firstName, lastName, programId) VALUES((SELECT login_Id FROM user_login WHERE username = @username), @firstName, @lastName, @program);", newUser);

                var userId = cnn.Query<string>("SELECT user_Id FROM user_profile WHERE login_Id IN (SELECT login_id FROM user_login WHERE username = @username);", newUser).First();
                var program_courses = cnn.Query<dynamic>("SELECT termNumber, courseName, courseCode, units, academicUnit FROM program_courses WHERE programCode = @programCode", new { programCode });

                foreach (var course in program_courses)
                {
                    DynamicParameters gradeParameters = new DynamicParameters();
                    gradeParameters.Add("@userId", userId);
                    gradeParameters.Add("@termNumber", course.termNumber);
                    gradeParameters.Add("@courseName", course.courseName);
                    gradeParameters.Add("@courseCode", course.courseCode);
                    gradeParameters.Add("@units", course.units);
                    gradeParameters.Add("@academicUnit", course.academicUnit);

                    cnn.Execute("INSERT into grade (userId, termNumber, courseName, courseCode, units, academicUnit) values (@userId, @termNumber, @courseName, @courseCode, @units, @academicUnit)", gradeParameters);
                }

                //DynamicParameters gradeParameters = new DynamicParameters();
                //cnn.Execute("INSERT into grades (termNumber, courseName, courseCode, units) values (@userId, @termNumber, @courseName, @courseCode, @units)", gradeParameters);
                //program_courses.ToList();

            }
        }

        public static string GetProgramId(string program)
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<string>("SELECT programId FROM Program WHERE programCODE = @program", new { program });
                    return output.First();
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

        public static bool VerifyLogin(User_LoginModel login)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                List<string> usernameMatch = cnn.Query<string>("SELECT username from user_login WHERE username = @username", login).ToList();

                if (usernameMatch.Count == 1)
                {
                    List<string> passwordMatch = cnn.Query<string>("SELECT password FROM user_login WHERE username = @username", login).ToList();

                    if (passwordMatch.Count == 1)
                    {
                        if(login.hashedPassword == passwordMatch.First())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
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
