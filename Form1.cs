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
        public GPAware()
        {
            InitializeComponent();
        }

        private void ShowPanel(Panel panelToShow)
        {
            // Hide all panels first DAGDAG SI GPATRACK
            CALCULATOR.Visible = false;
            HOME.Visible = false;
            TopDashboard.Visible = false;
            SIGNUP.Visible = true;
            GPAEDIT.Visible = false;

            // Show the selected panel
            panelToShow.Visible = true;
        }



        private void GPACalculator_Click(object sender, EventArgs e)
        {
            ShowPanel(GPAEDIT);
        }

        private void CreateAccountSignupBox_Click(object sender, EventArgs e)
        {
            NewUserModel newUser = new NewUserModel();
            string username = UsernameSignupBox.Text;
            string firstName = FirstNameSignupBox.Text;
            string lastName = LastNameSignupBox.Text;
            string program = ProgramSignupBox.Text;
            bool autoGenerateCourses = AutoGenerateCourses.Checked;
            string password = PasswordSignupBox.Text;
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
                            newUser.username = username;
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
                        newUser.firstName = firstName;
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
                        newUser.lastName = lastName;
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
                newUser.program = program;
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
                            newUser.password = password;
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
                //Add new user account
                SqliteDataAccess.AddUser(newUser);
                SuccessLabel.Text = "Account creation successful";
                SuccessLabel.Visible = true;
                CreateAccountSignupBox.Visible = false;

            }
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
            User_LoginModel newLogin = new User_LoginModel();

            newLogin.username = UsernameHomeLogin.Text;
        }

        private void CreateAccountRedirect_Click(object sender, EventArgs e)
        {
            ShowPanel(SIGNUP);
        }
    }
}
