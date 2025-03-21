using Dapper;
using DatabaseExperimentSOFSEC1Project;
using GradeCalculator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SOFSEC1_Project
{
    public class SqliteDataAccess
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        public static string GetLoginId(string username)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@username", username);
                var output = cnn.Query<string>("SELECT login_Id FROM user_login WHERE username = @username", parameters);
                return output.First();
            }
        }
        public static string GetUserId(string loginId)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@loginId", loginId);
                var output = cnn.Query<string>("SELECT user_Id FROM user_profile WHERE login_Id = @loginId;", parameters);
                return output.First();
            }
        }
        public static User_ProfileModel GetUserProfile(string userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@userId", userId);
                var encryptedProfile = cnn.Query<User_ProfileModel>("SELECT * from user_profile WHERE user_Id = @userId;", parameters);
                return encryptedProfile.First();
            }
        }

        public static List<GradeModel> GetGrades(string userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var encryptedGrades = cnn.Query<GradeModel>("SELECT * from grade WHERE userId = @userId;", new DynamicParameters(new { userId }));
                return encryptedGrades.ToList();
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
                    int userIdInput = Convert.ToInt32(userId);
                    string termNumberInput = course.termNumber;
                    string courseNameInput = course.courseName;
                    string courseCodeInput = course.courseCode;
                    string unitsInput = course.units;
                    string gradeInput = "N/A";
                    string academincUnitInput = course.academicUnit;

                    string termNumberOutput = GPAwareCryptography.Encrypt(password, termNumberInput);
                    string courseNameOutput = GPAwareCryptography.Encrypt(password, courseNameInput);
                    string courseCodeOutput = GPAwareCryptography.Encrypt(password, courseCodeInput);
                    string unitsOutput = GPAwareCryptography.Encrypt(password, unitsInput);
                    string gradeOutput = GPAwareCryptography.Encrypt (password, gradeInput);
                    string academincUnitOutput = GPAwareCryptography.Encrypt(password, academincUnitInput);

                    DynamicParameters gradeParameters = new DynamicParameters();
                    gradeParameters.Add("@userId", userIdInput);
                    gradeParameters.Add("@termNumber", termNumberOutput);
                    gradeParameters.Add("@courseName", courseNameOutput);
                    gradeParameters.Add("@courseCode", courseCodeOutput);
                    gradeParameters.Add("@units", unitsOutput);
                    gradeParameters.Add("@grade", gradeOutput);
                    gradeParameters.Add("@academicUnit", academincUnitOutput);

                    cnn.Execute("INSERT into grade (userId, termNumber, courseName, courseCode, units, grade, academicUnit) values (@userId, @termNumber, @courseName, @courseCode, @units, @grade, @academicUnit)", gradeParameters);
                }
            }
        }

        public static void UpdateGrades(List<GradeModel> grades, string userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                foreach (GradeModel grade in grades)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@userId", userId);
                    parameters.Add("@gradeId", grade.gradeId);
                    parameters.Add("@grade", grade.grade);
                    cnn.Execute("UPDATE grade SET grade = @grade WHERE userId = @userId AND gradeId = @gradeId", parameters);
                }
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
                var output = cnn.Query<string>("SELECT username from User_login", new DynamicParameters());

                return output.ToList();
            }
        }

        public static bool VerifyLogin(string username, string password)
        {
            username = username.Trim();
            password = password.Trim();
            List<string> usernameList = GetUsernames();

            if (GetUsernames().Contains(username))
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    string output = cnn.Query<string>("SELECT password FROM user_login WHERE username = @username", new { username }).First();

                    if (String.IsNullOrEmpty(password))
                    {
                        return false;
                    }
                    else
                    {
                        GPAwareCryptography crypto = new GPAwareCryptography();
                        if (crypto.VerifyPassword(password, output))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
            }
            else
            {
                return false;
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

        public static string GetProgramName(string programCode)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@programCode", programCode);
                var output = cnn.Query<string>("SELECT programName FROM Program WHERE programCode = @programCode", parameters);

                return output.First();
            }
        }

    }
}
