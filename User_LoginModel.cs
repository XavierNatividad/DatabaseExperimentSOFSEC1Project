using DatabaseExperimentSOFSEC1Project;
using GradeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOFSEC1_Project
{
    public class User_LoginModel
    {
        public string loginId { get; set; }
        public string userId { get; set; }
        public string username { get; set; }
       
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string programId { get; set; }

        public List<GradeModel> grades = new List<GradeModel>();

        public User_LoginModel()
        { } 
        public User_LoginModel(string username, string password)
        { 
            this.username = username.Trim();
            this.password = password.Trim();

            loginId = SqliteDataAccess.GetLoginId(this.username).ToString();
            userId = SqliteDataAccess.GetUserId(loginId.Trim()).ToString();

            User_ProfileModel profile = SqliteDataAccess.GetUserProfile(this.userId);

            DecryptProfile(profile);
            grades = SqliteDataAccess.GetGrades(this.userId);
        }
        public void DecryptProfile(User_ProfileModel profile)
        {
            firstName = GPAwareCryptography.Decrypt(password, profile.firstName);
            lastName = GPAwareCryptography.Decrypt(password, profile.lastName);
            programId = SqliteDataAccess.GetProgramName(GPAwareCryptography.Decrypt(password, profile.programId));
        }
        public void DecryptGrades()
        {
            foreach (GradeModel grade in grades)
            {
                grade.courseName = GPAwareCryptography.Decrypt(password, grade.courseName);
                grade.grade = GPAwareCryptography.Decrypt(password, grade.grade);
            }
        }
        public string[] totalUnits()
        {
            int acaemicUnitsCount = 0;
            int nonAcademicUnitsCount = 0;
            foreach (GradeModel grade in grades)
            {
                if(grade.academicUnit == "TRUE" && grade.grade != null)
                {
                    acaemicUnitsCount += Convert.ToInt32(grade.units);
                }
                if (grade.academicUnit == "FALSE" && grade.grade != null)
                {
                    nonAcademicUnitsCount += Convert.ToInt32(grade.units);
                }
            }
            string[] totalUnits = { acaemicUnitsCount.ToString(), nonAcademicUnitsCount.ToString(), (acaemicUnitsCount + nonAcademicUnitsCount).ToString() };
            return totalUnits;
        }
        public string CGPA()
        {
            bool empty = true;
            double totalGradePoints = 0;
            double totalUnits = 0;
            foreach (GradeModel grade in grades)
            {
                if (grade.academicUnit == "TRUE" && grade.grade != null)
                {
                    totalGradePoints += Convert.ToDouble(grade.grade) * Convert.ToDouble(grade.units);
                    totalUnits += Convert.ToDouble(grade.units);
                    empty = false;
                }
            }
            if (empty)
            {
                return "N/A";
            }
            else
            {
                return (totalGradePoints / totalUnits).ToString("0.00");
            }        
        }
        public void SaveGrades(DataGridView gradesTable)
        {
            grades = new List<GradeModel>();

            foreach (DataGridViewRow row in gradesTable.Rows)
            {

                GradeModel grade = new GradeModel
                {
                    gradeId = Convert.ToInt32(row.Cells["GradeId"].Value),
                    termNumber = row.Cells["TermNumber"].Value?.ToString(),
                    courseName = row.Cells["CourseName"].Value?.ToString(),
                    courseCode = row.Cells["CourseCode"].Value?.ToString(),
                    units = row.Cells["Units"].Value?.ToString(),
                    grade = row.Cells["Grade"].Value?.ToString(),
                    academicUnit = row.Cells["AcademicUnit"].Value?.ToString()
                };

                grades.Add(grade);
            }

            SqliteDataAccess.UpdateGrades(grades, userId);
            SqliteDataAccess.GetGrades(userId);

        }
        public void ClearGrades(DataGridView gradesTable)
        {
            grades = new List<GradeModel>();

            foreach (DataGridViewRow row in gradesTable.Rows)
            {

                GradeModel grade = new GradeModel
                {
                    gradeId = Convert.ToInt32(row.Cells["GradeId"].Value),
                    termNumber = row.Cells["TermNumber"].Value?.ToString(),
                    courseName = row.Cells["CourseName"].Value?.ToString(),
                    courseCode = row.Cells["CourseCode"].Value?.ToString(),
                    units = row.Cells["Units"].Value?.ToString(),
                    grade = "NULL",
                    academicUnit = row.Cells["AcademicUnit"].Value?.ToString()
                };

                grades.Add(grade);
            }

            SqliteDataAccess.UpdateGrades(grades, userId);
            SqliteDataAccess.GetGrades(userId);

        }
        public string HonorsStanding()
        {
            bool hasFailingGrade = false;
            foreach (GradeModel grade in grades)
            {
                if (grade.academicUnit == "TRUE" && grade.grade != null)
                {
                    if (Convert.ToDouble(grade.grade) < 1.0)
                    {
                        hasFailingGrade = true;
                    }
                }
            }
            if (CGPA() == "N/A")
            {
                return "There are currently no records of your grades available.";
            }
            else
            {
                double cgpa = Convert.ToDouble(CGPA());
                if (cgpa >= 3.8 && !hasFailingGrade)
                {
                    return "You are current on track to graduate as a Summa Cum Laude.";
                }
                else if (cgpa >= 3.6 && !hasFailingGrade)
                {
                    return "You are current on track to graduate as a Summa Cum Laude.";
                }
                else if (cgpa >= 3.4 && !hasFailingGrade)
                {
                    return "You are current on track to graduate as a Cum Laude.";
                }
                else if (cgpa >= 3.0 && !hasFailingGrade)
                {
                    return "You are current on track to graduate with Honorable Mentions.";
                }
                else
                {
                    return "You are current on track to graduate without honors.";
                }
            }
        }
        //public string DeansList()
        //{
        //    double lowestGrade = 4.0;
        //    foreach (GradeModel grade in grades)
        //    {
        //        if (grade.academicUnit == "TRUE" && grade.grade != null)
        //        {
        //            if (Convert.ToDouble(grade.grade) < lowestGrade)
        //            {
        //                lowestGrade = Convert.ToDouble(grade.grade);
        //            }
        //        }
        //    }
        //}
    }
}
