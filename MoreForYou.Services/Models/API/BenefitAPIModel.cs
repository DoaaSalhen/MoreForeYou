using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class BenefitAPIModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string BenefitCard { get; set; }

        public int Times { get; set; }

        public string BenefitType { get; set; }

        public bool EmployeeCanRedeem { get; set; }

        /// Data to used in Redeem
        public bool IsAgift { get; set; }

        public int MinParticipant { get; set; }

        public int MaxParticipant { get; set; }

        public string RequiredDocuments { get; set; }

        [Required]
        public int numberOfDays { get; set; }

        public string DateToMatch { get; set; }
        public DateTime CertainDate { get; set; }

        public string LastStatus { get; set; }
        public int TimesEmployeeReceiveThisBenefit { get; set; }

        public List<string> BenefitWorkflows { get; set; }
        //public List<string> BenefitConditions { get; set; }

        public List<BenefitStats> benefitStatses { get; set; }

        public Dictionary<string, string> BenefitConditions { get; set; }
        public Dictionary<string, bool> BenefitApplicable { get; set; }

    }

    public class BenefitStats
    {
        public string Name { get; set; }
        public int Count { get; set; }

    }

    public class BenefitConditionsAndAvailable
    {
        public Dictionary<string, string> BenefitConditions { get; set; }

        public Dictionary<string, bool> BenefitApplicable { get; set; }


    }

}
