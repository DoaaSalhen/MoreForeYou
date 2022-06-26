﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class NotificationAPIModel
    {
        public long EmployeeNumber { get; set; }

        public string EmployeeFullName { get; set; }

        public string EmployeeProfilePicture { get; set; }


        public string NotificationType { get; set; }
        public string Message { get; set; }

        public string RequestStatus { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

    }
}
