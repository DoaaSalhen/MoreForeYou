using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Models.Models.MasterModels
{
    public class RequestWorkflow
    {
        [Required]
        public BenefitRequest BenefitRequest { get; set; }
        [Key]
        public long BenefitRequestId { get; set; }
        [Required]
        public Employee Employee { get; set; }
        [Key]
        public long EmployeeId { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public int RequestStatusId { get; set; }
        [Required]
        public RequestStatus RequestStatus { get; set; }

        public string Notes { get; set; }

        public DateTime ReplayDate { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
