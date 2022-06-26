using MoreForYou.Models.Models;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreForYou.Services.Models.API
{
    public class HomeModel
    {


        public List<BenefitAPIModel> AllBenefitModels { get; set; }
        public List<BenefitAPIModel> AvailableBenefitModels { get; set; }

        public LoginUser user { get; set; }


    }
}
