using DatabaseExperimentSOFSEC1Project;
using GradeCalculator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            grades = SqliteDataAccess.GetGrades(this.userId);
            grades = DecryptGrades(grades);
            DecryptProfile(profile);
        }
        public void DecryptProfile(User_ProfileModel profile)
        {
            firstName = GPAwareCryptography.Decrypt(password, profile.firstName);
            lastName = GPAwareCryptography.Decrypt(password, profile.lastName);
            programId = SqliteDataAccess.GetProgramName(GPAwareCryptography.Decrypt(password, profile.programId));
        }
        public List<GradeModel> DecryptGrades(List<GradeModel> inputGrades)
        {
            List<GradeModel> decryptedGrades = new List<GradeModel>();
            foreach (GradeModel grade in inputGrades)
            {
                GradeModel newGrade = new GradeModel();
                newGrade.gradeId = grade.gradeId;
                newGrade.termNumber = GPAwareCryptography.Decrypt(password, grade.termNumber);
                newGrade.courseName = GPAwareCryptography.Decrypt(password, grade.courseName);
                newGrade.courseCode = GPAwareCryptography.Decrypt(password, grade.courseCode);
                newGrade.units = GPAwareCryptography.Decrypt(password, grade.units);
                newGrade.grade = GPAwareCryptography.Decrypt(password, grade.grade);
                newGrade.academicUnit = GPAwareCryptography.Decrypt(password, grade.academicUnit);
                decryptedGrades.Add(newGrade);
            }
            
            return decryptedGrades;
        }
        public string[] totalUnits()
        {
            int acaemicUnitsCount = 0;
            int nonAcademicUnitsCount = 0;
            foreach (GradeModel grade in grades)
            {
                if(grade.academicUnit == "TRUE" && grade.grade != "N/A")
                {
                    acaemicUnitsCount += Convert.ToInt32(grade.units);
                }
                if (grade.academicUnit == "FALSE" && grade.grade != "N/A")
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
                if (grade.academicUnit == "TRUE" && grade.grade != "N/A")
                {
                    if(grade.grade == "R")
                    {
                        totalGradePoints += Convert.ToDouble(0) * Convert.ToDouble(grade.units);
                        totalUnits += Convert.ToDouble(grade.units);
                    }
                    else 
                    {
                        totalGradePoints += Convert.ToDouble(grade.grade) * Convert.ToDouble(grade.units);
                        totalUnits += Convert.ToDouble(grade.units);
                    }
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
        public string TermGPA(int termNumber)
        {
            bool empty = true;
            double totalGradePoints = 0;
            double totalUnits = 0;
            foreach (GradeModel grade in grades)
            {
                if (grade.academicUnit == "TRUE" && grade.grade != "N/A" && int.Parse(grade.termNumber) == termNumber)
                {
                    if (grade.grade == "R")
                    {
                        totalGradePoints += Convert.ToDouble(0) * Convert.ToDouble(grade.units);
                        totalUnits += Convert.ToDouble(grade.units);
                    }
                    else
                    {
                        totalGradePoints += Convert.ToDouble(grade.grade) * Convert.ToDouble(grade.units);
                        totalUnits += Convert.ToDouble(grade.units);
                    }
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

            List<GradeModel> encryptedGrades = new List<GradeModel>();
            foreach (GradeModel grade in grades)
            {
                GradeModel newGrade = new GradeModel();
                newGrade.gradeId = grade.gradeId;
                newGrade.termNumber = GPAwareCryptography.Encrypt(password, grade.termNumber);
                newGrade.courseName = GPAwareCryptography.Encrypt(password, grade.courseName);
                newGrade.units = GPAwareCryptography.Encrypt(password, grade.units);
                newGrade.grade = GPAwareCryptography.Encrypt(password, grade.grade);
                newGrade.academicUnit = GPAwareCryptography.Encrypt(password, grade.academicUnit);
                encryptedGrades.Add(newGrade);
            }
            SqliteDataAccess.UpdateGrades(encryptedGrades, userId);
            DecryptGrades(SqliteDataAccess.GetGrades(userId));
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
                    grade = "N/A",
                    academicUnit = row.Cells["AcademicUnit"].Value?.ToString()
                };

                grades.Add(grade);
            }

            List<GradeModel> encryptedGrades = new List<GradeModel>();
            foreach (GradeModel grade in grades)
            {
                GradeModel newGrade = new GradeModel();
                newGrade.gradeId = grade.gradeId;
                newGrade.termNumber = GPAwareCryptography.Encrypt(password, grade.termNumber);
                newGrade.courseName = GPAwareCryptography.Encrypt(password, grade.courseName);
                newGrade.units = GPAwareCryptography.Encrypt(password, grade.units);
                newGrade.grade = GPAwareCryptography.Encrypt(password, grade.grade);
                newGrade.academicUnit = GPAwareCryptography.Encrypt(password, grade.academicUnit);
                encryptedGrades.Add(newGrade);
            }
            SqliteDataAccess.UpdateGrades(encryptedGrades, userId);
            DecryptGrades(SqliteDataAccess.GetGrades(userId));
        }
        public string HonorsStanding()
        {
            bool hasFailingGrade = false;
            foreach (GradeModel grade in grades)
            {
                if (grade.academicUnit == "TRUE" && grade.grade != "N/A")
                {
                    if (grade.grade == "R")
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
                    return "You are current on track to graduate without honors once you retake your failed classes.";
                }
            }
        }
        public string DeansList()
        {
            string CGPA = this.CGPA();
            if (CGPA == "N/A")
            {
                return "No records of your grades are available to determine if you are qualified to be on the Dean's List.";
            }
            else
            {
                string DeansListMessage = "You are eligible to a Dean's Lister for the following terms: \n";
                for (int i = 1; i < GetHighestTermNumber(); i++)
                {
                    int flowcharttUnits = FlowchartUnits(i);
                    int takenUnits = TakenUnits(i);

                    double lowestGrade = 0;

                    var filteredGrades = grades
                        .Where(g => g.termNumber == i.ToString())
                        .Where(g => g.grade != "N/A");

                    if (filteredGrades.Any())
                    {
                        lowestGrade = filteredGrades.Min(g => Convert.ToDouble(g.grade));
                    }
                    else
                    {
                        lowestGrade = -1;
                    }

                    if (lowestGrade != 1 && (lowestGrade >= 2.5 && takenUnits >= Math.Min(flowcharttUnits, 15)))
                    {
                        string termGPA = TermGPA(i);
                        if (termGPA != "N/A")
                        {
                            termGPA = Convert.ToDouble(termGPA).ToString("0.00");
                            if (Convert.ToDouble(termGPA) >= 3.5)
                            {
                                DeansListMessage += $"\tTerm {i.ToString("00")} | {termGPA} | First Honors \n";
                            }
                            else if (Convert.ToDouble(termGPA) >= 3.0)
                            {
                                DeansListMessage += $"\tTerm {i.ToString("00")} | {termGPA} | Second Honors \n";
                            }
                        }
                    }
                }
                return DeansListMessage;
            }
            
        }
        private int GetHighestTermNumber()
        {
            if (grades == null || grades.Count == 0)
            {
                return -1; // Return -1 if there are no grades  
            }

            int highestTermNumber = grades
                .Where(g => !string.IsNullOrEmpty(g.termNumber))
                .Max(g => int.Parse(g.termNumber));

            return highestTermNumber;
        }
        private int FlowchartUnits(int termNumber)
        {
            return grades
                .Where(g => g.termNumber == termNumber.ToString())
                .Sum(g => int.Parse(g.units));
        }
        private int TakenUnits(int termNumber)
        {
            return grades
                .Where(g => g.termNumber == termNumber.ToString())
                .Where(g => g.grade != "N/A")
                .Sum(g => int.Parse(g.units));
        }

        
            
    }
}
