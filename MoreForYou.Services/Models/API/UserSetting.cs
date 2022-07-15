using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class UserSetting
    {
        public string oldPassword { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{8,32}$")]
        public string newPassword { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{8,32}$")]
        [Compare("newPassword")]
        public string confirmPassword { get; set; }

        public long employeeNumber { get; set; }
        //public string imagePath { get; set; }

        //public string  userId { get; set; }

        //public string employeeName { get; set; }

    }
}
