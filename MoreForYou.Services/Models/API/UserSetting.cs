using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
   public class UserSetting
    {
        public string oldPassword { get; set; }

        public string newPassword { get; set; }

        public string confirmPassword { get; set; }


        public string imagePath { get; set; }

        public string  userId { get; set; }

        public string employeeName { get; set; }

    }
}
