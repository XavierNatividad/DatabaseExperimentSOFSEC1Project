using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseExperimentSOFSEC1Project
{
    class GradeModel
    {
        public int gradeId { get; set; }
        public int userId {  get; set; }
        public int termNumber { get; set; }
        public string courseName { get; set; }
        public string courseCode { get; set; }
        public decimal units { get; set; }
        public decimal grade { get; set; }
    }
}
