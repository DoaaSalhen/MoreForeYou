﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
   public class CompanyModel
    {

        [Required]
        public string Code { get; set; }

        public int Id { get; set; }
        [Required]
        public bool IsDelted { get; set; }
        [Required]
        public bool IsVisible { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }

    }
}
