using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class Gift
    {
        public long RequestNumber { get; set; }

        public long EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string BenefitName { get; set; }
        public string BenefitCard { get; set; }
        public string EmployeeDepartment { get; set; }
        public string EmployeeEmail { get; set; }
        public DateTime Date { get; set; }
    }
}
