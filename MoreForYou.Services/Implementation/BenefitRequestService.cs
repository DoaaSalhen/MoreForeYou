using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class BenefitRequestService : IBenefitRequestService
    {
        private readonly IRepository<BenefitRequest, long> _repository;
        private readonly IRepository<BenefitRequest, RequestWorkflow> _requestWorflowRepository;

        private readonly ILogger<BenefitRequestService> _logger;
        private readonly IMapper _mapper;


        public BenefitRequestService(IRepository<BenefitRequest, long> benefitRequestRepository,
          ILogger<BenefitRequestService> logger,
          IMapper mapper,
          IRepository<BenefitRequest, RequestWorkflow> requestWorflowRepository
           )
        {
            _repository = benefitRequestRepository;
            _logger = logger;
            _mapper = mapper;
            _requestWorflowRepository = requestWorflowRepository;
        }
        public BenefitRequestModel CreateBenefitRequest(BenefitRequestModel model)
        {
            BenefitRequest benefitRequest = _mapper.Map<BenefitRequest>(model);

            try
            {
                var addedBenefitReuest = _repository.Add(benefitRequest);
                if (addedBenefitReuest != null)
                {
                    BenefitRequestModel addedBenefiRequesttModel = new BenefitRequestModel();
                    addedBenefiRequesttModel = _mapper.Map<BenefitRequestModel>(addedBenefitReuest);
                    return addedBenefiRequesttModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public bool DeleteBenefitRequest(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BenefitRequestModel>> GetAllBenefitRequests()
        {
            throw new NotImplementedException();
        }

        public Task<List<BenefitRequestModel>> GetBenefitRequestByBenefitId(long benefitId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BenefitRequestModel>> GetBenefitRequestByEmployeeId(long employeeNumber)
        {
            try
            {
                List<BenefitRequest> benefitRequests = _repository.Find(r => r.EmployeeId == employeeNumber, false, r => r.Employee, r => r.Benefit, r => r.RequestStatus).ToList();
                List<BenefitRequestModel> benefitRequestModels = _mapper.Map<List<BenefitRequestModel>>(benefitRequests);
                return benefitRequestModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
            return null;

        }

        public BenefitRequestModel GetBenefitRequest(long id)
        {
            try
            {
                BenefitRequest benefitRequest = _repository.Find(b => b.Id == id && b.IsVisible == true, false, r => r.Benefit, r => r.Employee, r => r.RequestStatus).First();
                BenefitRequestModel benefitRequestModel = _mapper.Map<BenefitRequestModel>(benefitRequest);
                return benefitRequestModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<bool> UpdateBenefitRequest(BenefitRequestModel model)
        {
            bool result = false;
            try
            {
                BenefitRequest benefitRequest = _mapper.Map<BenefitRequest>(model);
                result = _repository.Update(benefitRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return Task<bool>.FromResult<bool>(result);
        }

        public bool CancelBenefitRequest(BenefitRequestModel benefitRequestModel, RequestWokflowModel requestWokflowModel)
        {
            bool result = false;
            try
            {
                BenefitRequest benefitRequest = _mapper.Map<BenefitRequest>(benefitRequestModel);
                RequestWorkflow requestWorkflow = _mapper.Map<RequestWorkflow>(requestWokflowModel);

                var canelledBenefitRequest = _requestWorflowRepository.UpdateTwoEntities(benefitRequest, requestWorkflow);
                if (canelledBenefitRequest != null)
                {
                    result = true;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return result;
        }


        public async Task<List<BenefitRequestModel>> GetBenefitRequestByEmployeeId(long employeeNumber, long benefitId)
        {
            try
            {
                List<BenefitRequest> benefitRequests = _repository.Find(r => r.EmployeeId == employeeNumber && r.BenefitId == benefitId, false, r => r.Employee, r => r.Employee.Department, r => r.Employee.Position, r => r.Employee.Company, r => r.Benefit, r => r.Benefit.BenefitType, r => r.RequestStatus).ToList();
                List<BenefitRequestModel> benefitRequestModels = _mapper.Map<List<BenefitRequestModel>>(benefitRequests);
                return benefitRequestModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<BenefitRequestModel> GetBenefitRequestByGroupId(long groupId)
        {
            try
            {
                BenefitRequest benefitRequest = _repository.Find(r => r.GroupId == groupId && r.IsVisible == true, false, r => r.Benefit, r => r.Benefit.BenefitType, r => r.RequestStatus, r => r.Group, r => r.Employee, r => r.Employee.Department, r => r.Employee.Position, r => r.Employee.Company).First();
                BenefitRequestModel benefitRequestModel = _mapper.Map<BenefitRequestModel>(benefitRequest);
                return benefitRequestModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public int GetTimesEmployeeReceieveThisBenefit(long employeeNumber, long benefitId)
        {
            try
            {
                int times = _repository.Find(r => r.EmployeeId == employeeNumber && r.BenefitId == benefitId && r.RequestStatusId == (int)CommanData.BenefitStatus.Approved).ToList().Count;
                return times;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return -1;
            }
        }

        public bool ISEmployeeHasHoldingRequestsForthisBenefit(long employeeNumber, long benefitId)
        {
            try
            {
                var times = _repository.Find(r => r.EmployeeId == employeeNumber && r.BenefitId == benefitId && (r.RequestStatusId == (int)CommanData.BenefitStatus.Pending || r.RequestStatusId == (int)CommanData.BenefitStatus.InProgress));
                if (times.Count() > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }
        //public AddIndividualBenefitRequest(Request request)
        //{
        //    try
        //    {
        //        BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
        //        EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(request.EmployeeNumber).Result;
        //        BenefitRequestModel NewBenefitRequestModel = new BenefitRequestModel();
        //        BenefitModel benefitModel = _BenefitService.GetBenefit(benefitRequestModel.Benefit.Id);
        //        List<BenefitModel> benefitModels = new List<BenefitModel>();
        //        string fileName = "";
        //        benefitModels.Add(benefitModel);
        //        benefitModels = _EmployeeService.BenefitsUserCanRedeem(benefitModels).Result;
        //        if (benefitModels.First().EmployeeCanRedeem == true)
        //        {
        //            if (benefitRequestModel.Benefit.BenefitTypeId == 2)
        //            {
        //                NewBenefitRequestModel.EmployeeId = CurrentEmployee.EmployeeNumber;
        //                NewBenefitRequestModel.Message = benefitRequestModel.Message;
        //                NewBenefitRequestModel.CreatedDate = DateTime.Now;
        //                NewBenefitRequestModel.RequestDate = DateTime.Now;
        //                NewBenefitRequestModel.UpdatedDate = DateTime.Now;
        //                NewBenefitRequestModel.IsVisible = true;
        //                NewBenefitRequestModel.IsDelted = false;
        //                NewBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //                NewBenefitRequestModel.BenefitId = benefitRequestModel.Benefit.Id;
        //                NewBenefitRequestModel.ExpectedDateFrom = benefitRequestModel.ExpectedDateFrom;
        //                if (benefitRequestModel.ExpectedDateTo == null)
        //                {
        //                    NewBenefitRequestModel.ExpectedDateTo = benefitRequestModel.ExpectedDateFrom;
        //                }
        //                else
        //                {
        //                    NewBenefitRequestModel.ExpectedDateTo = benefitRequestModel.ExpectedDateTo;

        //                }
        //                if (benefitRequestModel.RequiredDocumentsfiles != null)
        //                {
        //                    foreach (var file in benefitRequestModel.RequiredDocumentsfiles)
        //                    {
        //                        fileName = UploadedFile(file, "BenefitRequestFiles") + ";" + fileName;
        //                    }
        //                }
        //                if (benefitRequestModel.SendTo == 0)
        //                {
        //                    benefitRequestModel.SendTo = CurrentEmployee.EmployeeNumber;
        //                }
        //                BenefitRequestModel addedRequest = _benefitRequestService.CreateBenefitRequest(NewBenefitRequestModel);

        //                if (addedRequest != null)
        //                {
        //                    BenefitRequestModel newBenefitRequestModel = new BenefitRequestModel();
        //                    newBenefitRequestModel.Benefit = _BenefitService.GetBenefit(addedRequest.BenefitId);
        //                    if (newBenefitRequestModel.Benefit.HasWorkflow)
        //                    {

        //                        string Message = SendReuestToWhoIsConcern(addedRequest.Id, 1).Result;
        //                        ViewBag.Message = Message;

        //                    }

        //                }
        //                else
        //                {
        //                    ViewBag.Message = "Sorry, you request can't be created";

        //                }
        //            }
        //        }
        //    catch(Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //    }
        //}


        public string CancelMyRequest(long id)
        {
            bool cancelResult = false;
            string message = "";
            BenefitRequestModel DBBenefitRequestModel = GetBenefitRequest(id);
            if (DBBenefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)
            {
                RequestWokflowModel requestWokflowModel = new RequestWokflowModel();  //_requestWorkflowService.GetRequestWorkflow(id);
                if (requestWokflowModel != null)
                {
                    RequestWokflowModel requestWokflowModel1 = requestWokflowModel;
                    requestWokflowModel1.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    DBBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    cancelResult = CancelBenefitRequest(DBBenefitRequestModel, requestWokflowModel1);
                }
                else
                {
                    DBBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    cancelResult = UpdateBenefitRequest(DBBenefitRequestModel).Result;
                }

                if (cancelResult == true)
                {
                    message = "Success Process";
                }
            }
            else
            {
                message = "your request can not be Cancelled";
            }
            return message;
        }



        public Request CreateRequestAPIModel(BenefitRequestModel benefitRequestModel)
        {
            try
            {
                Request request = new Request();
                request.benefitId = benefitRequestModel.BenefitId;
                request.BenefitName = benefitRequestModel.BenefitName;
                request.EmployeeNumber = benefitRequestModel.Employee.EmployeeNumber;
                request.CanCancel = benefitRequestModel.CanCancel;
                request.CanEdit = benefitRequestModel.CanEdit;
                request.From = benefitRequestModel.ExpectedDateFrom.ToString("yyyy-MM-dd");
                request.To = benefitRequestModel.ExpectedDateTo.ToString("yyyy-MM-dd");
                request.RequestStatusId = benefitRequestModel.RequestStatusId;
                if (benefitRequestModel.Participants == null)
                {
                    request.ParticipantsData = new List<Participant>();
                }
                else
                {
                    //request.ParticipantsData = benefitRequestModel.Participants;
                }
                if (benefitRequestModel.BenefitId == 3)
                {
                    request.GroupName = benefitRequestModel.Group.Name;
                }
                request.Message = benefitRequestModel.Message;
                request.RequestWorkFlowAPIs = new List<RequestWorkFlowAPI>();
                if (benefitRequestModel.RequestWokflowModels != null)
                {
                    foreach (var workflow in benefitRequestModel.RequestWokflowModels)
                    {
                        RequestWorkFlowAPI requestWorkFlowAPI = new RequestWorkFlowAPI();
                        requestWorkFlowAPI.EmployeeName = workflow.Employee.FullName;
                        requestWorkFlowAPI.EmployeeNumber = workflow.EmployeeId;
                        requestWorkFlowAPI.Notes = workflow.Notes;
                        requestWorkFlowAPI.ReplayDate = workflow.ReplayDate;
                        requestWorkFlowAPI.Status = Enum.GetName(typeof(CommanData.BenefitStatus), workflow.RequestStatusId);

                        request.RequestWorkFlowAPIs.Add(requestWorkFlowAPI);
                    }
                }
                return request;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        //public string AddNewRequest (BenefitRequestModel benefitRequestModel, string userId)
        //{
        //    EmployeeModel CurrentEmployee = _employeeService.GetEmployeeByUserId(userId).Result;

        //    BenefitModel benefitModel = _benefitService.GetBenefit(benefitRequestModel.Benefit.Id);
        //    List<BenefitModel> benefitModels = new List<BenefitModel>();
        //    string fileName = "";
        //    string result = "";
        //    benefitModels.Add(benefitModel);
        //    benefitModels = _benefitService.BenefitsUserCanRedeem(benefitModels, CurrentEmployee);
        //    if (benefitModels.First().EmployeeCanRedeem == true)
        //    {
        //        if (benefitRequestModel.Benefit.BenefitTypeId == 2)
        //        {
        //            AddIndividualRequest(benefitRequestModel, CurrentEmployee.EmployeeNumber)
        //        }
        //    }
        //    else
        //    {
        //        result = "Sorry, you can't redeem to this benefit now";

        //    }
        //    return result;
        //}

        public Request CreateRequestModel(RequestAPI requestAPI, long benefitTypeId, bool isGift)
        {
            Request request = new Request();
            request.benefitId = requestAPI.benefitId;
            request.EmployeeNumber = requestAPI.EmployeeNumber;
            request.benefitId = requestAPI.benefitId;
            request.EmployeeNumber = requestAPI.EmployeeNumber;
            request.From = requestAPI.From;

            request.To = requestAPI.To;
            if (request.Message != "")
            {
                request.Message = requestAPI.Message;
            }
            if (isGift == true)
            {
                request.SendToId = (long)requestAPI.SendToId;

            }
            if (requestAPI.Documents.Length > 0)
            {
                request.Documents = requestAPI.Documents;
            }
            if (benefitTypeId == (int)CommanData.BenefitTypes.Group)
            {
                request.GroupName = requestAPI.GroupName;
                request.SelectedEmployeeNumbers = requestAPI.SelectedEmployeeNumbers;
            }
            return request;
        }

        public List<BenefitRequestModel> GetRequestsSendToMe(long employeeNumber)
        {
            try
            {
                var requests = _repository.Find(r => r.SendTo == employeeNumber, false, r => r.RequestStatus, r => r.Benefit, r => r.Employee, r => r.Employee.Department);
                if (requests != null)
                {
                    List<BenefitRequestModel> benefitRequestModels = _mapper.Map<List<BenefitRequestModel>>(requests);
                    return benefitRequestModels;
                }
                else
                {
                    return new List<BenefitRequestModel>();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
