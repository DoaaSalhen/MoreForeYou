using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBenefitRequestService
    {
        Task<List<BenefitRequestModel>> GetAllBenefitRequests();
        BenefitRequestModel CreateBenefitRequest(BenefitRequestModel model);
        Task<bool> UpdateBenefitRequest(BenefitRequestModel model);
        bool DeleteBenefitRequest(long id);
        BenefitRequestModel GetBenefitRequest(long id);
        public Task<List<BenefitRequestModel>> GetBenefitRequestByEmployeeId(long employeeNumber);
        Task<List<BenefitRequestModel>> GetBenefitRequestByBenefitId(long benefitId);
        bool CancelBenefitRequest(BenefitRequestModel benefitRequestModel, RequestWokflowModel requestWokflowModel);
        public Task<List<BenefitRequestModel>> GetBenefitRequestByEmployeeId(long employeeNumber, long benefitId);
        Task<BenefitRequestModel> GetBenefitRequestByGroupId(long groupId);
        int GetTimesEmployeeReceieveThisBenefit(long employeeNumber, long benefitId);
        Request CreateRequestAPIModel(BenefitRequestModel benefitRequestModel);
        //Task<string> SendReuestToWhoIsConcern(long benefitRequetId, int orderNumber);

    }
}
