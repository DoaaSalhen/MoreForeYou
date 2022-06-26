using Microsoft.AspNetCore.Http;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBenefitService
    {
        Task<List<BenefitModel>> GetAllBenefits();
        BenefitModel CreateBenefit(BenefitModel model);
        Task<bool> UpdateBenefit(BenefitModel model);
        bool DeleteBenefit(long id);
        BenefitModel GetBenefit(long id);
        Task<List<BenefitModel>> GetBenefitByName(BenefitModel model);
        List<BenefitModel> BenefitsUserCanRedeem(List<BenefitModel> benefitModels, EmployeeModel employeeModel);
        List<string> CreateBenefitConditions(BenefitModel benefitModel);
        BenefitAPIModel CreateBenefitAPIModel(BenefitModel model);
        public List<BenefitAPIModel> GetMyBenefits(long employeeNumber);

        List<Participant> GetEmployeesCanRedeemThisGroupBenefit(long employeeNumber, long benefitId);

        HomeModel ShowAllBenefits(EmployeeModel employeeModel);
        BenefitAPIModel GetBenefitDetails(long benefitId, EmployeeModel employeeModel);
        List<Participant> GetEmployeesWhoCanIGiveThisBenefit(long employeeNumber, long benefitId);
        Request BenefitRedeem(long benefitId, string userId);

    }
}
