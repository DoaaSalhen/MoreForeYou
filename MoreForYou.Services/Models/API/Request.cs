using Microsoft.AspNetCore.Http;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class Request
    {
        public long RequestNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public string Message { get; set; }

        public string GroupName { get; set; }

        public string Participants { get; set; }

        public long SendToId { get; set; }
        public LoginUser SendToModel { get; set; }

        public long benefitId { get; set; }

        public string BenefitName { get; set; }

        public string BenefitType { get; set; }

        public long EmployeeNumber { get; set; }

        public int RequestStatusId { get; set; }

        public bool CanCancel { get; set; }

        public bool CanEdit { get; set; }

        public string status { get; set; }

        public LoginUser CreatedBy { get; set; }
        public string WarningMessage { get; set; }

        public bool EmployeeCanResponse { get; set; }
        public string Requestedat { get; set; }

        public List<RequestWorkFlowAPI> RequestWorkFlowAPIs { get; set; }

        public List<Participant> ParticipantsData { get; set; }
        public List<LoginUser> FullParticipantsData { get; set; }
        //public IFormFile[] Documents { get; set; }

        public int numberOfDays { get; set; }

        public string DateToMatch { get; set; }

        public int Year { get; set; }

        public int MaxParticipant { get; set; }

        public int MinParticipant { get; set; }

        public string[] RequiredDocuments { get; set; }

        public bool IsAgift { get; set; }

        public string[] DocumentsPath { get; set; }
    }

    public class RequestWorkFlowAPI
    {
        public long EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string Status { get; set; }

        public DateTime ReplayDate { get; set; }

        public string Notes { get; set; }

        public bool employeeCanResponse { get; set; }
    }

    public class MyRequests
    {
        public List<Request> Requests { get; set; }
    }

    //public class RequestRedeem
    //{
    //    public DateTime From { get; set; }

    //    public DateTime To { get; set; }

    //    public int MyProperty { get; set; }

    //}

}
