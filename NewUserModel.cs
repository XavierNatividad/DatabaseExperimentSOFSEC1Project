﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOFSEC1_Project
{
    public class NewUserModel
    {
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string program { get; set; }
        public string password { get; set; }

        public NewUserModel()
        { }
        public NewUserModel(string username, string firstName, string lastName, string program, string password) 
        {
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.program = program;
            this.password = password;
        }
    }
}
