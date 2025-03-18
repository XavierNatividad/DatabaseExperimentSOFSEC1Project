using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOFSEC1_Project
{
    class UserRegistrationModel
    {
        private string username { get; set; }
        private string firstName { get; set; }
        private string lastName { get; set; }
        private string program { get; set; }
        private string password { get; set; }
        private string confirmPassword { get; set; }
        private bool autoGenerateCourses { get; set; }

        public UserRegistrationModel()
        { }
        public UserRegistrationModel(string username, string firstName, string lastName, string program, string password, string confirmPassword, bool autoGenerateCourses) 
        {
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.program = program;
            this.password = password;
            this.confirmPassword = confirmPassword;
            this.autoGenerateCourses = autoGenerateCourses;
        }
    }
}
