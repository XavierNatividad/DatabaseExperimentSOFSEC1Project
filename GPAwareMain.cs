using DatabaseExperimentSOFSEC1Project;
using GradeCalculator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SOFSEC1_Project
{
    public partial class GPAware: Form
    {
        User_LoginModel userLogin = new User_LoginModel();
        public GPAware()
        {
            InitializeComponent();
            ShowPanel(HOME);
            // TO START WITH HOME ALWAYS

        }

        private void ShowPanel(Panel panelToShow)
        {
            // Hide all panels
            HOME.Visible = false;
            SIGNUP.Visible = false;
            DASHBOARD.Visible = false;
            GPAEDIT.Visible = false;
            GPAVIEW.Visible = false;
            PROFILE.Visible = false;

            // Show the selected panel
            panelToShow.Visible = true;
        }

        private void CreateAccountSignupBox_Click(object sender, EventArgs e)
        {
            NewUserModel newUser = new NewUserModel();
            GPAwareCryptography cryptography = new GPAwareCryptography();
            string username = UsernameSignupBox.Text;
            string firstName = FirstNameSignupBox.Text;
            string lastName = LastNameSignupBox.Text;
            string program = ProgramSignupBox.Text;
            string password = PasswordSignupBox.Text.Trim();
            string confirmPassword = ConfirmPasswordSignupBox.Text;

            bool usernamePassed = false;
            bool fnPassed = false;
            bool lnPassed = false;
            bool programPassed = false;
            bool passwordPassed = false;

            //Username
            if (!string.IsNullOrWhiteSpace(username))
            {
                if (Regex.IsMatch(username, "^[a-zA-Z0-9_]+$"))
                {
                    if (username.Length >= 8 && username.Length <= 20)
                    {
                        List<string> existingUsernames = SqliteDataAccess.GetUsernames();
                        if (!existingUsernames.Contains(username))
                        {
                            //Username passed
                            usernamePassed = true;
                        }
                        else
                        {
                            //Username is already taken
                            usernamePassed = false;
                            InvalidUsernameLabel.Text = "Username is already taken";
                            InvalidUsernameLabel.Visible = true;
                        }
                    }
                    else
                    {
                        //Username must be between 8 and 20 characters
                        usernamePassed = false;
                        InvalidUsernameLabel.Text = "Username must be between 8 and 20 characters";
                        InvalidUsernameLabel.Visible = true;

                    }
                }
                else
                {
                    //Username can only contain letters, numbers, and underscores
                    usernamePassed = false;
                    InvalidUsernameLabel.Text = "Username can only contain letters, numbers, and underscores";
                    InvalidUsernameLabel.Visible = true;
                } 
            }
            else
            {
                //Username must not be empty
                usernamePassed = false;
                InvalidUsernameLabel.Text = "Username must not be empty";
                InvalidUsernameLabel.Visible = true;
            }

            //First name
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                if (firstName.Length <= 64)
                {
                    if (Regex.IsMatch(firstName, "^[a-zA-Z ]+$"))
                    {
                        //First name passed
                        fnPassed = true;
                    }
                    else
                    {
                        //First name can only contain letters and spaces
                        fnPassed = false;
                        InvalidFirstNameLabel.Text = "First name can only contain letters and spaces";
                        InvalidFirstNameLabel.Visible = true;
                    }
                }
                else
                {
                    //First name must be up to 64 characters
                    fnPassed = false;
                    InvalidFirstNameLabel.Text = "First name must be up to 64 characters";
                    InvalidFirstNameLabel.Visible = true;
                }
            }
            else
            {
                //First name must not be empty
                fnPassed = false;
                InvalidFirstNameLabel.Text = "First name must not be empty";
                InvalidFirstNameLabel.Visible = true;
            }

            //Last name
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                if (lastName.Length <= 64)
                {
                    if (Regex.IsMatch(lastName, "^[a-zA-Z ]+$"))
                    {
                        //Last name passed
                        lnPassed = true;
                    }
                    else
                    {
                        //Last name can only contain letters and spaces
                        lnPassed = false;
                        InvalidLastNameLabel.Text = "Last name can only contain letters and spaces";
                        InvalidLastNameLabel.Visible = true;
                    }
                }
                else
                {
                    //Last name must be up to 64 characters
                    lnPassed = false;
                    InvalidLastNameLabel.Text = "Last name must be up to 64 characters";
                    InvalidLastNameLabel.Visible = true;
                }
            }
            else
            {
                //Last name must not be empty
                InvalidLastNameLabel.Text = "Last name must not be empty";
                InvalidLastNameLabel.Visible = true;
            }

            //Program
            if (!string.IsNullOrWhiteSpace(program))
            {
                programPassed = true;
            }
            else
            {
                programPassed = false;
                InvalidProgramLabel.Text = "No program was selected";
                InvalidProgramLabel.Visible = true;
            }

            //Password
            if (!string.IsNullOrWhiteSpace(password))
            {
                if (password.Length >= 8 && password.Length <= 64)
                {

                    if (Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()\-_=+])[A-Za-z\d!@#$%^&*()\-_=+]{8,64}$"))
                    {
                        if (password == confirmPassword)
                        {
                            //Password passed
                            passwordPassed = true;
                        }
                        else
                        {
                            //Passwords do not match
                            passwordPassed = false;
                            InvalidPasswordLabel.Text = "Passwords do not match";
                            InvalidPasswordLabel.Visible = true;
                        }
                    }
                    else
                    {
                        //Password must contain at least one letter, one number, and one special character
                        passwordPassed = false;
                        InvalidPasswordLabel.Text = "Password must contain at least one letter, one number, and one special character";
                        InvalidPasswordLabel.Visible = true;
                    }
                }
                else
                {
                    //Password must be between 8 and 64 characters
                    passwordPassed = false;
                    InvalidPasswordLabel.Text = "Password must be between 8 and 64 characters";
                    InvalidPasswordLabel.Visible = true;
                }

            }
            else
            {
                //Password must not be empty
                passwordPassed = false;
                InvalidPasswordLabel.Text = "Password must not be empty";
                InvalidPasswordLabel.Visible = true;
            }

            if (usernamePassed && passwordPassed && fnPassed && lnPassed && programPassed)
            {
                Cursor.Current = Cursors.WaitCursor;
                ReturnToLoginButton.Visible = false;
                AccountCreationLabel.Visible = true;
                CreateAccountSignupBox.Visible = false;;
                
                //Add new user account
                newUser.username = username;
                newUser.password = GPAwareCryptography.BytesArrayToString(cryptography.HashPassword(password, cryptography.CreateSalt()));
                
                firstName = GPAwareCryptography.Encrypt(password, firstName);
                newUser.firstName = firstName;

                lastName = GPAwareCryptography.Encrypt(password, lastName);
                newUser.lastName = lastName;

                program = GPAwareCryptography.Encrypt(password, program);
                newUser.program = program;

                SqliteDataAccess.AddUser(newUser, password, GPAwareCryptography.Decrypt(password, program));

                AccountCreationLabel.Visible = false;
                ReturnToLoginButton.Visible = true;
                Cursor.Current = Cursors.Default;
                SuccessLabel.Text = "Account creation successful!"; 
                SuccessLabel.Visible = true;
            }
            newUser = null;
            newUser = null;
            cryptography = null;
            username = null;
            firstName = null;
            lastName = null;
            program = null;
            password = null;
            confirmPassword = null;

            usernamePassed = false;
            fnPassed = false;
            lnPassed = false;
            programPassed = false;
            passwordPassed = false;
        }

        private void UsernameSignupBox_TextChanged(object sender, EventArgs e)
        {
            InvalidUsernameLabel.Visible = false;
        }

        private void FirstNameSignupBox_TextChanged(object sender, EventArgs e)
        {
            InvalidFirstNameLabel.Visible = false;
        }

        private void PasswordSignupBox_TextChanged(object sender, EventArgs e)
        {
            InvalidPasswordLabel.Visible = false;
        }

        private void ConfirmPasswordSignupBox_TextChanged(object sender, EventArgs e)
        {
            InvalidPasswordLabel.Visible = false;
        }

        private void ProgramSignupBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvalidProgramLabel.Visible = false;
        }

        private void ProgramSignupBox_DropDown(object sender, EventArgs e)
        {
            List<string> programList = new List<string>();
            programList = SqliteDataAccess.GetPrograms();

            foreach (string program in programList)
            {
                ProgramSignupBox.Items.Add(program);
            }
        }

        private void ReturnToLoginButton_Click(object sender, EventArgs e)
        {
            ShowPanel(HOME);
        }

        private void LoginHome_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string username = UsernameHomeLogin.Text;
            string password = PasswordHomeLogin.Text.Trim();

            if (SqliteDataAccess.VerifyLogin(username, password))
            {
                try
                {
                    userLogin = new User_LoginModel(username, password);
                    string GPA = userLogin.CGPA().ToString();
                    GPALabel.Text = GPA;
                    TrackDashboardText.Text = userLogin.HonorsStanding();

                    string[] units = userLogin.totalUnits();
                    AcademicUnitsValueLabel.Text = units[0];
                    NonAcademicUnitsValueLabel.Text = units[1];
                    TotalUnitsValueLabel.Text = units[2];
                    DeansListTitleLabel.Text = userLogin.DeansList();

                    UsernameHomeLogin.Text = "";
                    PasswordHomeLogin.Text = "";
                    ShowPanel(DASHBOARD);
                }
                catch (Exception)
                {
                    InvalidLoginLabel.Text = "An error occurred while logging in.";
                    InvalidLoginLabel.Visible = true;

                    UsernameHomeLogin.Text = "";
                    PasswordHomeLogin.Text = "";
                }
            }
            else
            {
                InvalidLoginLabel.Text = "Username or password is invalid.";
                InvalidLoginLabel.Visible = true;

                UsernameHomeLogin.Text = "";
                PasswordHomeLogin.Text = "";
            }
            Cursor.Current = Cursors.Default;
        }

        private void CreateAccountRedirect_Click(object sender, EventArgs e)
        {
            ShowPanel(SIGNUP);
        }

        private void DashboardProfile_Click(object sender, EventArgs e)
        {
            string GPA = userLogin.CGPA().ToString();
            GPALabel.Text = GPA;
            TrackDashboardText.Text = userLogin.HonorsStanding();

            string[] units = userLogin.totalUnits();
            AcademicUnitsValueLabel.Text = units[0];
            NonAcademicUnitsValueLabel.Text = units[1];
            TotalUnitsValueLabel.Text = units[2];
            DeansListTitleLabel.Text = userLogin.DeansList();
            ShowPanel(DASHBOARD);
        }

        private void CGPAProfile_Click(object sender, EventArgs e)
        {
            LoadGradesIntoDataGridView();
            ShowPanel(GPAVIEW);
        }

        private void NameProfile_Click(object sender, EventArgs e)
        {
            ShowPanel(PROFILE);
            UsernameReplaceProfile.Text = userLogin.username.ToString();

            FirstNameReplaceProfile.Text = userLogin.firstName.ToString();
            LastNameReplaceProfile.Text = userLogin.lastName.ToString();
            ProgramReplaceProfile.Text = userLogin.programId.ToString();

            //FirstNameBoxProfile.Text = GPAwareCryptography.Decrypt(userLogin.password, userLogin.firstName);
            //LastNameBoxProfile.Text = GPAwareCryptography.Decrypt(userLogin.password, userLogin.lastName);
            //ProgramBoxProfile.Text = SqliteDataAccess.GetProgramName(GPAwareCryptography.Decrypt(userLogin.password, userLogin.programId));
        }

        private void EditModeGPAView_Click(object sender, EventArgs e)
        {
            LoadGradesIntoDataGridView();
            ShowPanel(GPAEDIT);
        }

        private void UsernameHomeLogin_TextChanged(object sender, EventArgs e)
        {
            InvalidLoginLabel.Visible = false;
        }

        private void PasswordHomeLogin_TextChanged(object sender, EventArgs e)
        {
            InvalidLoginLabel.Visible = false;
        }

        private void GPAware_Load(object sender, EventArgs e)
        {
            SuccessLabel.Visible = false;
            AccountCreationLabel.Visible = false;
        }

        private void ReturnToLoginButton_Click_1(object sender, EventArgs e)
        {
            UsernameHomeLogin.Text = "";
            PasswordHomeLogin.Text = "";

            UsernameSignupBox.Text = "";
            FirstNameSignupBox.Text = "";
            LastNameSignupBox.Text = "";
            ProgramSignupBox.Text = "";
            PasswordSignupBox.Text = "";
            ConfirmPasswordSignupBox.Text = "";

            SuccessLabel.Visible = false;
            CreateAccountSignupBox.Visible = true;
            ShowPanel(HOME);
        }

        private void DASHBOARD_VisibleChanged(object sender, EventArgs e)
        {
            NameDashboardPopup.Text = userLogin.firstName + " " + userLogin.lastName;
        }

        private void GPAVIEW_VisibleChanged(object sender, EventArgs e)
        {
            //CGPAView.Text = GradeCalculator.CalculateCGPA(userLogin.grades).ToString();
        }

        private void SaveGPAEdit_Click(object sender, EventArgs e)
        {
            YesButtonClear.Visible = false;
            NoButtonClear.Visible = false;
            YesButtonDiscard.Visible = false;
            NoButtonDiscard.Visible = false;
            userLogin.SaveGrades(GradesTableEdit);

            AreYouSureLabel.Text = "Your changes have been saved.";
            AreYouSureLabel.Visible = true;
            
            LoadGradesIntoDataGridView();
        }

        private void GradeConversionTableButton_Click(object sender, EventArgs e)
        {
            GradeConversionTable gradeConversionTable = new GradeConversionTable();
            gradeConversionTable.Show();
        }

        private void DiscardGPAEdit_Click(object sender, EventArgs e)
        {
            AreYouSureLabel.ForeColor = Color.Black;
            AreYouSureLabel.Text = "Are you sure you want to discard all changes?";
            AreYouSureLabel.Visible = true;
            YesButtonDiscard.Visible = true;
            NoButtonDiscard.Visible = true;
        }
        private void DeleteGPAEdit_Click(object sender, EventArgs e)
        {
            AreYouSureLabel.ForeColor = Color.Red;
            AreYouSureLabel.Text = "Are you sure you want to clear all grades?";
            AreYouSureLabel.Visible = true;
            YesButtonClear.Visible = true;
            NoButtonClear.Visible = true;
        }

        private void NoButtonClear_Click(object sender, EventArgs e)
        {
            AreYouSureLabel.Visible = false;
            YesButtonClear.Visible = false;
            NoButtonClear.Visible = false;
        }
        private void YesButtonClear_Click(object sender, EventArgs e)
        {
            YesButtonClear.Visible = false;
            NoButtonClear.Visible = false;
            userLogin.ClearGrades(GradesTableEdit);
            LoadGradesIntoDataGridView();
            AreYouSureLabel.ForeColor = Color.Black;
            AreYouSureLabel.Text = "All grade entries have been cleared.";
            
        }
        private void NoButtonDiscard_Click(object sender, EventArgs e)
        {
            AreYouSureLabel.Visible = false;
            YesButtonDiscard.Visible = false;
            NoButtonDiscard.Visible = false;
        }
        private void YesButtonDiscard_Click(object sender, EventArgs e)
        {
            LoadGradesIntoDataGridView();
            AreYouSureLabel.Text = "Your changes have been discarded.";
            YesButtonDiscard.Visible = false;
            NoButtonDiscard.Visible = false;    
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainTabControl.SelectedTab == ConversionTable) // Ensure you're on the correct tab
            {
                ShowGradeConversionTable();
            }
        }

        private void ShowGradeConversionTable()
        {
            // Prevent multiple instances of the form inside the tab
            ConversionTable.Controls.Clear();

            GradeConversionTable gradeForm = new GradeConversionTable();
            gradeForm.TopLevel = false; 
            gradeForm.FormBorderStyle = FormBorderStyle.None; 
            gradeForm.Dock = DockStyle.Fill; 

            ConversionTable.Controls.Add(gradeForm);
            gradeForm.Show();
        }

        private void LogOutProfile_Click(object sender, EventArgs e)
        {
            LogoutConfirm confirmDialog = new LogoutConfirm(() =>
            {
                ShowPanel(HOME);
                            UsernameHomeLogin.Text = "";
            PasswordHomeLogin.Text = "";
            });

            if (confirmDialog.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void GradesTableEdit_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            AreYouSureLabel.Visible = false;
        }
        private void LoadGradesIntoDataGridView()
        {
            DataGridViewComboBoxColumn gradePicker = new DataGridViewComboBoxColumn();
            gradePicker.HeaderText = "Grade";
            gradePicker.Name = "Grade";

            gradePicker.Items.Add("4.0");
            gradePicker.Items.Add("3.5");
            gradePicker.Items.Add("3.0");
            gradePicker.Items.Add("2.5");
            gradePicker.Items.Add("2.0");
            gradePicker.Items.Add("1.5");
            gradePicker.Items.Add("1.0");
            gradePicker.Items.Add("R");
            gradePicker.Items.Add("N/A");

            GradesTableView.Rows.Clear();
            GradesTableView.Columns.Clear();
            GradesTableView.Columns.Add("GradeID", "Grade ID");
            GradesTableView.Columns.Add("TermNumber", "Term Number");
            GradesTableView.Columns.Add("CourseName", "Course Name");
            GradesTableView.Columns.Add("CourseCode", "Course Code");
            GradesTableView.Columns.Add("Units", "Units");
            GradesTableView.Columns.Add("AcademicUnit", "Academic Unit");
            GradesTableView.Columns.Add("Grade", "Grade");

            GradesTableEdit.Rows.Clear();
            GradesTableEdit.Columns.Clear();
            GradesTableEdit.Columns.Add("GradeID", "Grade ID");
            GradesTableEdit.Columns.Add("TermNumber", "Term Number");
            GradesTableEdit.Columns.Add("CourseName", "Course Name");
            GradesTableEdit.Columns.Add("CourseCode", "Course Code");
            GradesTableEdit.Columns.Add("Units", "Units");
            GradesTableEdit.Columns.Add("AcademicUnit", "Academic Unit");
            GradesTableEdit.Columns.Add(gradePicker);

            for (int i = 0; i < 6; i++)
            {
                GradesTableEdit.Columns[i].ReadOnly = true;
            }

            foreach (var grade in userLogin.grades)
            {
                GradesTableView.Rows.Add(grade.gradeId, grade.termNumber, grade.courseName, grade.courseCode, grade.units, grade.academicUnit, grade.grade);

                int rowIndex = GradesTableEdit.Rows.Add(grade.gradeId, grade.termNumber, grade.courseName, grade.courseCode, grade.units, grade.academicUnit);
                GradesTableEdit.Rows[rowIndex].Cells["Grade"].Value = grade.grade;
            }
        }

        private void GradesTableEdit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            AreYouSureLabel.Visible = false;
            YesButtonClear.Visible = false;
            NoButtonClear.Visible = false;
            YesButtonDiscard.Visible = false;
            NoButtonDiscard.Visible = false;
        }

        private void UsernameHomeLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 

                Control nextControl = this.GetNextControl((Control)sender, true);

                while (nextControl != null && !(nextControl is TextBox))
                {
                    nextControl = this.GetNextControl(nextControl, true);
                }

                if (nextControl != null)
                {
                    nextControl.Focus();
                }
            }
        }
    }
}
