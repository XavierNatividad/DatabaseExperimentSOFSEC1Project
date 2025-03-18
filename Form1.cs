using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            DASHBOARD.Visible = false;
            SIGNUP.Visible = true;
            GPAVIEW.Visible = false;

            // Show the selected panel
            panelToShow.Visible = true;
        }



        private void GPACalculator_Click(object sender, EventArgs e)
        {
            ShowPanel(GPAVIEW);
        }

        private void CreateAccountSignupBox_Click(object sender, EventArgs e)
        {
            UserRegistrationModel newUser = new UserRegistrationModel();

            ////username
            ////must be between 8-20 letters
            //if ((UsernameSignupBox.Text.Length >= 8 && UsernameSignupBox.Text.Length <= 20))
            //{
            //    //must only contain letters, numbers, and underscores
            //    if (UsernameSignupBox.Text.Contains(c =>))
            //    {
            //        //must be unique
            //        if ()
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else 
            //    {
                    
            //    }
            //}
            //else
            //{

            //}

            ////first name
            ////must not be blank
            //if ()
            //{
            //    //must not be 
            //}
            //else
            //{
            //}

            ////passwords must match
            //if (PasswordSignupBox.Text != ConfirmPasswordSignupBox.Text)
            //{

            //}
        }

        private void UsernameSignupBox_TextChanged(object sender, EventArgs e)
        {
            if ((UsernameSignupBox.Text.Length >= 8 && UsernameSignupBox.Text.Length <= 20))
            {
                toolTip1.Show("Must be between 8 and 20 characters", UsernameSignupBox);
            }
        }

        private void FirstNameSignupBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PasswordSignupBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConfirmPasswordSignupBox_TextChanged(object sender, EventArgs e)
        {

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
    }
}
