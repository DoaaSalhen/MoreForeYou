using Microsoft.AspNetCore.Http;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IRequestWorkflowService
    {
        List<RequestWokflowModel> GetAllRequestWorkflows();
        Task<RequestWokflowModel> CreateRequestWorkflow(RequestWokflowModel model);
        Task<bool> UpdateRequestWorkflow(RequestWokflowModel model);
        bool DeleteRequestWorkflow(long id);
        List<RequestWokflowModel> GetRequestWorkflowByEmployeeNumber(long employeeNumber);
        List<RequestWokflowModel> GetRequestWorkflow(long requestId);
        Task<bool> CancelRequestWorkflow(RequestWokflowModel requestWokflowModel);
        RequestWokflowModel GetRequestWorkflowByEmployeeNumber(long employeeNumber, long requestId);

        List<RequestWokflowModel> GetGroupRequestWorkflowByEmployeeNumber(long employeeNumber);
        Task<ManageRequest> CreateManageRequestFilter(string userId, ManageRequest manageRequest);
        List<Request> CreateRequestToApprove(List<RequestWokflowModel> requestWokflowModels);
        List<RequestWokflowModel> CreateWarningMessage(List<RequestWokflowModel> requestWokflowModels);
        List<RequestWokflowModel> EmployeeCanResponse(List<RequestWokflowModel> requestWokflowModels);
        string CancelMyRequest(long id);
        List<Request> GetMyBenefitRequests(long employeeNumber, long BenefitId, long benefitTypeId);
        List<Request> CreateRequestToApprove(List<BenefitRequestModel> benefitRequestModels, long employeeNumber);
        string AddIndividualRequest(Request request, string userId, BenefitModel benefitModel);
        Task<string> SendReuestToWhoIsConcern(long benefitRequetId, int orderNumber);
        Task<string> UploadedImageAsync(IFormFile ImageName, string path);
        bool SendGroupRequestToHR(BenefitRequestModel benefitRequestModel);
        Task<string> ConfirmGroupRequest(Request request, string userId, BenefitModel benefitModel);

        Task<bool> SendNotification(BenefitRequestModel benefitRequestModel, RequestWokflowModel DBRequestWorkflowModel, string type);

        Task<bool> SendNotificationToGroupMembers(List<GroupEmployeeModel> groupEmployeeModels, RequestWokflowModel DBRequestWorkflowModel, BenefitRequestModel benefitRequestModel, string notificationMessage, string type);

        NotificationModel CreateNotification(string Type, long employeeNumber, long BenefitRequestId, string message, long responsedBy);

        Task<bool> SendToSpecificUser(string message, RequestWokflowModel model, string requestType, string employeeName, string userId);

        Task<string> AddDocumentsToRequest(long requestNumber, List<IFormFile> files);
        ManageRequest CreateManageRequestModel(RequestSearch requestSearch);
        List<Gift> GetMyGifts(long employeeNumber);


    }
}
