using Microsoft.AspNetCore.Http;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class BenefitModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public BenefitTypeModel BenefitType { get; set; }

        [Required]
        public long BenefitTypeId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public bool HasWorkflow { get; set; }
        public int gender { get; set; }

        public int WorkDuration { get; set; }

        public int Age { get; set; }


        public string AgeSign { get; set; }

        public int MaritalStatus { get; set; }

        public int MinParticipant { get; set; }

        public int MaxParticipant { get; set; }

        public BenefitWorkflowModel BenefitWorkflow { get; set; }

        public List<BenefitWorkflowModel> BenefitWorkflowModels { get; set; }

        public List<RoleOrder> RolesOrder { get; set; }
        public List<GenderModel> genderModels { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public bool EmployeeCanRedeem { get; set; }
        public Dictionary<string, string> BenefitConditions { get; set; }
        public string MartialStatusText { get; set; }
        public string GenderText { get; set; }

        [Required]
        public string BenefitCard { get; set; }

        [NotMapped]
        [Required]
        public IFormFile ImageName { get; set; }


        public string LastStatus { get; set; }
        public DateTime LastRequetedDate { get; set; }

        [Required]
        public int Times { get; set; }

        [Required]
        public int SelectedCollarId { get; set; }

        public string RequiredDocuments { get; set; }

        [Required]
        public int numberOfDays { get; set; }

        public List<Collar> Collars { get; set; }

        [Required]
        public int Collar { get; set; }

        public List<string> AgeSigns { get; set; }

        public List<string> DatesToMatch { get; set; }

        public string DateToMatch { get; set; }
        public DateTime CertainDate { get; set; }
        public bool IsAgift { get; set; }
        public List<string> BenefitWorkflows { get; set; }

        public int NumberOfApprovedRequests { get; set; }

        public List<BenefitStats> benefitStatses { get; set; }
        public int BenefitReturn { get; set; }
        public Dictionary<string, bool> BenefitApplicable { get; set; }


    }

    public class RoleOrder
    {
        public int order { get; set; }
        public string RoleId { get; set; }

        public string RoleName { get; set; }


    }
}
