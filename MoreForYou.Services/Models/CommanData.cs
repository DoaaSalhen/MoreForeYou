using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models
{
    public class CommanData
    {
        public enum MaritialStatus
        {
            Single = 1,
            Married = 2,
            Divorced = 3,
            Any = -1
        };

        public enum Gender
        {
            Any = -1,
            Male = 1,
            Female = 2
        };

        public enum BenefitStatus
        {
            Pending = 1,
            InProgress = 2,
            Approved = 3,
            Rejected = 4,
            Cancelled = 5,
            NotStartedYet = 6
        };


        public enum CollarTypes
        {
            Any = -1,
            WhiteCollar = 1,
            blueCollar = 2,
        };

        public enum BenefitTypes
        {
            Individual = 1,
            Group = 2,
        };


        public List<GenderModel> genderList = new List<GenderModel>()
            {
                new GenderModel {  Name='M'},
                new GenderModel {  Name='F'}
            };

        public List<ResonseStatus> resonseStatuses = new List<ResonseStatus>()
        {
            new ResonseStatus {Id =-1, Name ="None"},
            new ResonseStatus {Id =1, Name ="Approve"},
            new ResonseStatus {Id =1, Name ="Disapprove"},
        };

        public static List<RequestStatusModelAPI> whoIsConcernRequestStatusModels = new List<RequestStatusModelAPI>()
        {
            new RequestStatusModelAPI {Id =-1, Name ="Status"},
            new RequestStatusModelAPI {Id =1, Name ="Pending"},
            new RequestStatusModelAPI {Id =3, Name ="Approved"},
            new RequestStatusModelAPI {Id =4, Name ="Rejected"},

        };

        public static List<RequestStatusModelAPI> RequestStatusModels = new List<RequestStatusModelAPI>()
        {
            new RequestStatusModelAPI {Id =2, Name ="InProgress"},
            new RequestStatusModelAPI {Id =1, Name ="Pending"},
            new RequestStatusModelAPI {Id =3, Name ="Approved"},
            new RequestStatusModelAPI {Id =4, Name ="Rejected"},
            new RequestStatusModelAPI {Id =5, Name ="Cancelled"},
            new RequestStatusModelAPI {Id =0, Name =""},



        };


        public static List<TimingModel> timingModels = new List<TimingModel>()
        {
             new TimingModel{Id=-1, Name="Date"},
             new TimingModel{Id=1, Name="Today"},
            new TimingModel{Id=2, Name="Last Day"},
            new TimingModel{Id=3, Name="Current Week"},
            new TimingModel{Id=4, Name="Current Month"},
        };

        public static List<Collar> Collars = new List<Collar>()
        {
               new Collar { Id = -1, Name = "Any" },
            new Collar { Id = 1, Name = "White Collar" },
            new Collar { Id = 2, Name = "Blue Collar" }

        };

        public static List<BenefitTypeModel> BenefitTypeModels = new List<BenefitTypeModel>()
        {
               new BenefitTypeModel { Id = -1, Name = "Any" },
            new BenefitTypeModel { Id = 2, Name = "Individual" },
            new BenefitTypeModel { Id = 3, Name = "Group" }

        };

        public List<string> AgeSigns = new List<string>()
        {
           ">",
           "<",
           "="
        };

        public List<string> DatesToMatch = new List<string>()
        {
            "Any",
           "Birth Date",
           "Join Date",
           "certain Date"
        };

    }

    public class ResonseStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Collar
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
