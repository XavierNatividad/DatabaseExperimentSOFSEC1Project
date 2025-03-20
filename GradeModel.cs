using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseExperimentSOFSEC1Project
{
    public class GradeModel
    {
        public int gradeId { get; set; }
        public int userId {  get; set; }
        public string termNumber { get; set; }
        public string courseName { get; set; }
        public string courseCode { get; set; }
        public string units { get; set; }
        public string grade { get; set; }
        public string academicUnit { get; set; }
    }
}
