using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.hub;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class RequestWorkflowService : IRequestWorkflowService
    {
        private readonly IRepository<RequestWorkflow, long> _repository;
        private readonly ILogger<RequestWorkflowService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _EmployeeService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        //private readonly IBenefitService _benefitService;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IBenefitTypeService _benefitTypeService;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IDepartmentService _departmentService;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IGroupService _groupService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;
        private readonly IRequestDocumentService _requestDocumentService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserConnectionManager _userConnectionManager;

        public RequestWorkflowService(IRepository<RequestWorkflow, long> requestWorkflowRepository,
          ILogger<RequestWorkflowService> logger,
          IMapper mapper,
          UserManager<AspNetUser> userManager,
          IEmployeeService EmployeeService,
          IUserService userService,
          IRoleService roleService,
          //IBenefitService benefitService,
          IRequestStatusService requestStatusService,
          IBenefitTypeService benefitTypeService,
          IBenefitRequestService benefitRequestService,
          IDepartmentService departmentService,
          IGroupEmployeeService groupEmployeeService,
          IGroupService groupService,
          IBenefitWorkflowService benefitWorkflowService,
          IRequestDocumentService requestDocumentService,
          IWebHostEnvironment hostEnvironment,
           IHubContext<NotificationHub> hub,
            INotificationService notificationService,
            IUserNotificationService userNotificationService,
            IUserConnectionManager userConnectionManager
          )
        {
            _repository = requestWorkflowRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _EmployeeService = EmployeeService;
            _userService = userService;
            _roleService = roleService;
            _requestStatusService = requestStatusService;
            _benefitTypeService = benefitTypeService;
            _benefitRequestService = benefitRequestService;
            _departmentService = departmentService;
            _groupEmployeeService = groupEmployeeService;
            _groupService = groupService;
            _benefitWorkflowService = benefitWorkflowService;
            _requestDocumentService = requestDocumentService;
            _hostEnvironment = hostEnvironment;
            _hub = hub;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _userConnectionManager = userConnectionManager;
        }
        public async Task<RequestWokflowModel> CreateRequestWorkflow(RequestWokflowModel model)
        {
            var requestWorkflow = _mapper.Map<RequestWorkflow>(model);

            try
            {
                var addedReuestWorkflow = _repository.Add(requestWorkflow);
                if (addedReuestWorkflow != null)
                {
                    RequestWokflowModel addedRequestWokflowModel = new RequestWokflowModel();
                    addedRequestWokflowModel = _mapper.Map<RequestWokflowModel>(addedReuestWorkflow);
                    return addedRequestWokflowModel;
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

        public bool DeleteRequestWorkflow(long id)
        {
            throw new NotImplementedException();
        }

        public List<RequestWokflowModel> GetAllRequestWorkflows()
        {
            List<RequestWorkflow> requestWorkflows = _repository.Find(rw => rw.IsVisible == true, false, RW => RW.Employee, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company).ToList();
            List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
            return requestWokflowModels;
        }

        public List<RequestWokflowModel> GetRequestWorkflow(long requestId)
        {
            try
            {
                List<RequestWorkflow> requestWorkflows = _repository.Find(RW => RW.BenefitRequestId == requestId, false, RW => RW.Employee, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company).ToList();
                List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<bool> UpdateRequestWorkflow(RequestWokflowModel model)
        {
            bool result = false;
            try
            {
                RequestWorkflow requestWorkflow = _mapper.Map<RequestWorkflow>(model);
                result = _repository.Update(requestWorkflow);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return result;

            }
        }
        public List<RequestWokflowModel> GetRequestWorkflow2(long requestId)
        {
            try
            {
                List<RequestWorkflow> requestWorkflows = _repository.Find(RW => RW.BenefitRequestId == requestId, false, RW => RW.Employee, RW => RW.RequestStatus).ToList();
                List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public Task<bool> CancelRequestWorkflow(RequestWokflowModel model)
        {
            bool result = false;
            try
            {
                RequestWorkflow requestWorkflow = _mapper.Map<RequestWorkflow>(model);
                result = _repository.Update(requestWorkflow);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(result);
        }

        public List<RequestWokflowModel> GetRequestWorkflowByEmployeeNumber(long employeeNumber)
        {
            try
            {
                List<RequestWorkflow> requestWorkflows = _repository.Find(RW => RW.EmployeeId == employeeNumber && RW.IsVisible == true, false, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Group, RW => RW.BenefitRequest.Group.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company).ToList();
                List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public RequestWokflowModel GetRequestWorkflowByEmployeeNumber(long employeeNumber, long requestId)
        {
            try
            {
                RequestWorkflow requestWorkflow = _repository.Find(RW => RW.EmployeeId == employeeNumber && RW.BenefitRequestId == requestId, false, RW => RW.BenefitRequest, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Benefit).First();
                RequestWokflowModel requestWokflowModel = _mapper.Map<RequestWokflowModel>(requestWorkflow);
                return requestWokflowModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<RequestWokflowModel> GetGroupRequestWorkflowByEmployeeNumber(long employeeNumber)
        {
            try
            {
                List<RequestWorkflow> requestWorkflows = _repository.Find(RW => RW.EmployeeId == employeeNumber && RW.IsVisible == true, false, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Group, RW => RW.BenefitRequest.Group.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company).ToList();
                List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }



        public async Task<ManageRequest> CreateManageRequestFilter(string userId)
        {
            try
            {
                ManageRequest manageRequest = new ManageRequest();
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userId).Result;
                UserModel userModel = await _userService.GetUser(userId);
                AspNetUser user = _mapper.Map<AspNetUser>(userModel);
                List<string> userRoles = _userManager.GetRolesAsync(user).Result.ToList();
                RequestFilterModel requestFilterModel = new RequestFilterModel();

                if (userRoles != null)
                {
                    manageRequest.BenefitTypeModels = CommanData.BenefitTypeModels;
                    manageRequest.RequestStatusModels = CommanData.whoIsConcernRequestStatusModels;
                    manageRequest.TimingModels = CommanData.timingModels;
                    manageRequest.SelectedDepartmentId = -1;
                    manageRequest.SelectedRequestStatus = -1;
                    manageRequest.SelectedTimingId = -1;
                    manageRequest.SelectedBenefitType = -1;
                    manageRequest.employeeNumberSearch = 0;
                    if (userRoles.Contains("Admin"))
                    {
                        manageRequest.DepartmentModels = _departmentService.GetAllDepartments();
                        manageRequest.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Department" });
                    }

                }
                return manageRequest;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public List<Request> CreateRequestToApprove(List<RequestWokflowModel> requestWokflowModels)
        {
            try
            {
                int ListCount = requestWokflowModels.Count;
                List<Request> requestsToApprove = new List<Request>();

                if (requestWokflowModels.Count != 0)
                {
                    for (int index = 0; index < requestWokflowModels.Count; index++)
                    {
                        Request requestToApprove1 = new Request();
                        requestToApprove1.benefitId = requestWokflowModels[index].BenefitRequest.BenefitId;
                        requestToApprove1.BenefitName = requestWokflowModels[index].BenefitRequest.Benefit.Name;
                        requestToApprove1.RequestNumber = requestWokflowModels[index].BenefitRequestId;
                        requestToApprove1.From = requestWokflowModels[index].BenefitRequest.ExpectedDateFrom.ToString("yyyy-MM-dd");
                        requestToApprove1.To = requestWokflowModels[index].BenefitRequest.ExpectedDateTo.ToString("yyyy-MM-dd");
                        requestToApprove1.Requestedat = requestWokflowModels[index].BenefitRequest.CreatedDate.ToString("yyyy-MM-dd");
                        requestToApprove1.Message = requestWokflowModels[index].BenefitRequest.Message;
                        requestToApprove1.status = CommanData.RequestStatusModels.Where(s => s.Id == requestWokflowModels[index].BenefitRequest.RequestStatusId).First().Name;
                        requestToApprove1.BenefitType = requestWokflowModels[index].BenefitRequest.Benefit.BenefitType.Name;
                        requestToApprove1.EmployeeCanResponse = requestWokflowModels[index].canResponse;

                        if (requestWokflowModels[index].BenefitRequest.WarningMessage != null)
                        {
                            requestToApprove1.WarningMessage = requestWokflowModels[index].BenefitRequest.WarningMessage;
                        }
                        if (requestWokflowModels[index].DocumentsPath != null)
                        {
                            requestToApprove1.DocumentsPath = requestWokflowModels[index].DocumentsPath;
                        }
                        List<EmployeeModel> employeeModels1 = new List<EmployeeModel>();
                        employeeModels1.Add(requestWokflowModels[index].BenefitRequest.Employee);
                        requestToApprove1.CreatedBy = CreateEmployeeData(employeeModels1).First();
                        if (requestWokflowModels[index].BenefitRequest.Group != null)
                        {
                            requestToApprove1.GroupName = requestWokflowModels[index].BenefitRequest.Group.Name;
                            List<EmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)requestWokflowModels[index].BenefitRequest.GroupId).Result.Select(eg => eg.Employee).ToList();
                            if (groupEmployeeModels != null)
                            {
                                List<LoginUser> employeesData = CreateEmployeeData(groupEmployeeModels);
                                requestToApprove1.FullParticipantsData = employeesData;
                            }
                        }
                        if (requestWokflowModels[index].BenefitRequest.SendTo != 0)
                        {
                            EmployeeModel employeeModel = _EmployeeService.GetEmployee(requestWokflowModels[index].BenefitRequest.SendTo);
                            List<EmployeeModel> employeeModels = new List<EmployeeModel>();
                            employeeModels.Add(employeeModel);
                            requestToApprove1.SendToModel = CreateEmployeeData(employeeModels).First();
                        }
                        requestsToApprove.Add(requestToApprove1);
                    }
                }
                return requestsToApprove;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<LoginUser> CreateEmployeeData(List<EmployeeModel> groupEmployeeModels)
        {
            try
            {
                List<LoginUser> employeesData = new List<LoginUser>();
                groupEmployeeModels.ForEach(eg => eg.Email = _userManager.FindByIdAsync(eg.UserId).Result.Email);
                foreach (var groupEmployeeModel in groupEmployeeModels)
                {
                    LoginUser employeeData = new LoginUser();
                    employeeData.EmployeeNumber = groupEmployeeModel.EmployeeNumber;
                    employeeData.EmployeeName = groupEmployeeModel.FullName;
                    employeeData.DepartmentName = groupEmployeeModel.Department.Name;
                    employeeData.PositionName = groupEmployeeModel.Position.Name;
                    employeeData.SapNumber = groupEmployeeModel.SapNumber;
                    employeeData.PhoneNumber = groupEmployeeModel.PhoneNumber;
                    employeeData.JoinDate = groupEmployeeModel.JoiningDate.ToString("yyyy-MM-dd");
                    employeeData.BirthDate = groupEmployeeModel.BirthDate.ToString("yyyy-MM-dd");
                    employeeData.Email = groupEmployeeModel.Email;
                    employeeData.Collar = CommanData.Collars.Where(c => c.Id == groupEmployeeModel.Collar).First().Name;
                    employeeData.Company = groupEmployeeModel.Company.Code;
                    employeeData.WorkDuration = CalculateWorkDuration(Convert.ToDateTime(employeeData.JoinDate));
                    employeesData.Add(employeeData);
                }

                return employeesData;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        //public EmployeeData CreateEmployeeData(GroupEmployeeModel groupEmployeeModel)
        //{
        //    try
        //    {
        //        EmployeeData employeeData = new EmployeeData();
        //        employeeData.EmployeeNumber = groupEmployeeModel.EmployeeId;
        //        employeeData.EmployeeName = groupEmployeeModel.Employee.FullName;
        //        employeeData.DepartmentName = groupEmployeeModel.Employee.Department.Name;
        //        employeeData.PositionName = groupEmployeeModel.Employee.Position.Name;
        //        employeeData.SapNumber = groupEmployeeModel.Employee.SapNumber;
        //        employeeData.PhoneNumber = groupEmployeeModel.Employee.PhoneNumber;
        //        employeeData.JoinDate = groupEmployeeModel.Employee.JoiningDate.ToString("yyyy-MM-dd");
        //        employeeData.BirthDate = groupEmployeeModel.Employee.BirthDate.ToString("yyyy-MM-dd");
        //        employeeData.Email = groupEmployeeModel.Employee.Email;
        //        employeeData.Collar = CommanData.Collars[groupEmployeeModel.Employee.Collar].Name;
        //        employeeData.Company = groupEmployeeModel.Employee.Company.Code;

        //        return employeeData;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //        return null;
        //    }
        //}

        public List<RequestWokflowModel> CreateWarningMessage(List<RequestWokflowModel> requestWokflowModels)
        {
            try
            {
                foreach (var request in requestWokflowModels)
                {
                    if (request.BenefitRequest.Benefit.DateToMatch != null)
                    {
                        if (request.BenefitRequest.Benefit.DateToMatch == "Birth Date")
                        {
                            if (request.BenefitRequest.ExpectedDateFrom.Day != request.BenefitRequest.Employee.BirthDate.Day || request.BenefitRequest.ExpectedDateFrom.Month != request.BenefitRequest.Employee.BirthDate.Month)
                            {
                                request.BenefitRequest.WarningMessage = "Employee Birth Date does not match requird Date";
                            }
                        }
                        if (request.BenefitRequest.Benefit.DateToMatch == "Join Date")
                        {
                            if (request.BenefitRequest.ExpectedDateFrom.Day != request.BenefitRequest.Employee.JoiningDate.Day || request.BenefitRequest.ExpectedDateFrom.Month != request.BenefitRequest.Employee.JoiningDate.Month)
                            {
                                request.BenefitRequest.WarningMessage = "Employee Join Date does not match requird Date";
                            }
                        }
                    }
                    else if (request.BenefitRequest.Benefit.CertainDate != null)
                    {
                        if (request.BenefitRequest.ExpectedDateFrom.Day != request.BenefitRequest.Benefit.CertainDate.Day || request.BenefitRequest.ExpectedDateFrom.Month != request.BenefitRequest.Benefit.CertainDate.Month)
                        {
                            request.BenefitRequest.WarningMessage = "Required date does not match benefit date";
                        }
                    }
                }

                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<RequestWokflowModel> EmployeeCanResponse(List<RequestWokflowModel> requestWokflowModels)
        {
            bool canResponse = false;
            foreach (RequestWokflowModel employeeRequestWokflowModel in requestWokflowModels)
            {
                if (employeeRequestWokflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending && (employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Today && employeeRequestWokflowModel.BenefitRequest.RequestStatusId != (int)CommanData.BenefitStatus.Approved || employeeRequestWokflowModel.BenefitRequest.RequestStatusId != (int)CommanData.BenefitStatus.Rejected))
                {
                    canResponse = true;
                }
                else
                {
                    canResponse = false;

                }

                employeeRequestWokflowModel.canResponse = canResponse;
            }
            return requestWokflowModels;
        }

        public string CalculateWorkDuration(DateTime joinDate)
        {
            string workDuration = "";
            TimeSpan diff = DateTime.Today - joinDate;
            int days = diff.Days;
            int months = days / 30;
            int years = (int)(days / (356.25));
            if (years < 1)
            {
                if (months < 1)
                {
                    workDuration = days + " days";
                }
                else
                {
                    workDuration = months + " Months";
                }

            }
            else
            {
                workDuration = years + " years";

            }
            return workDuration;
        }

        public string CancelMyRequest(long id)
        {
            bool cancelResult = false;
            string message = "";
            BenefitRequestModel DBBenefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
            if (DBBenefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)
            {
                var requestWokflowModel = GetRequestWorkflow(id);
                if (requestWokflowModel != null)
                {
                    RequestWokflowModel requestWokflowModel1 = requestWokflowModel.First();
                    requestWokflowModel1.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    DBBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    cancelResult = _benefitRequestService.CancelBenefitRequest(DBBenefitRequestModel, requestWokflowModel1);
                }
                else
                {
                    DBBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    cancelResult = _benefitRequestService.UpdateBenefitRequest(DBBenefitRequestModel).Result;
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


        public List<Request> GetMyBenefitRequests(long employeeNumber, long BenefitId, long benefitTypeId)
        {
            try
            {
                List<BenefitRequestModel> benefitRequestModels = new List<BenefitRequestModel>();
                if (benefitTypeId == (int)CommanData.BenefitTypes.Individual)
                {
                    benefitRequestModels = _benefitRequestService.GetBenefitRequestByEmployeeId(employeeNumber, BenefitId).Result;

                }
                else if (benefitTypeId == (int)CommanData.BenefitTypes.Group)
                {
                    List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupsByEmployeeId(employeeNumber).Result;
                    List<GroupModel> groupModels = groupEmployeeModels.Where(g => g.Group.BenefitId == BenefitId).Select(g => g.Group).ToList();
                    foreach (GroupModel group in groupModels)
                    {
                        BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
                        benefitRequestModel = _benefitRequestService.GetBenefitRequestByGroupId(group.Id).Result;
                        benefitRequestModel.CurrentMember = _EmployeeService.GetEmployee(employeeNumber);
                        benefitRequestModels.Add(benefitRequestModel);
                    }
                }
                benefitRequestModels = benefitRequestModels.OrderByDescending(r => r.CreatedDate).ToList();
                List<Request> myRequests = new List<Request>();
                if (benefitRequestModels != null)
                {
                    string HRRoleId = _roleService.GetRoleByName("HR").Result.Id;
                    foreach (BenefitRequestModel model in benefitRequestModels)
                    {
                        //if (model.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled)
                        //{
                        model.RequestWokflowModels = GetRequestWorkflow(model.Id);
                        var HRWorkflow = model.RequestWokflowModels.Where(rw => rw.RoleId == HRRoleId);
                        if (HRWorkflow.Any())
                        {
                            var HRmodel = model.RequestWokflowModels.Where(rw => rw.RoleId.Contains(HRRoleId)).First();
                            HRmodel.Employee.FullName = "HR";
                            model.RequestWokflowModels.RemoveAll(rw => rw.RoleId == HRRoleId);
                            model.RequestWokflowModels.Add(HRmodel);
                        }
                        //}
                        if (model.RequestStatusId == (int)CommanData.BenefitStatus.Pending)
                        {
                            model.CanCancel = true;
                            model.CanEdit = true;
                        }
                        else
                        {
                            model.CanCancel = false;
                            model.CanEdit = false;
                        }
                    }
                    myRequests = CreateRequestToApprove(benefitRequestModels, employeeNumber);
                }
                return myRequests;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<Request> CreateRequestToApprove(List<BenefitRequestModel> benefitRequestModels, long employeeNumber)
        {
            try
            {
                int ListCount = benefitRequestModels.Count;
                List<Request> requestsToApprove = new List<Request>();

                if (benefitRequestModels.Count != 0)
                {
                    for (int index = 0; index < benefitRequestModels.Count; index++)
                    {
                        Request requestToApprove1 = new Request();
                        requestToApprove1.benefitId = benefitRequestModels[index].BenefitId;
                        requestToApprove1.BenefitName = benefitRequestModels[index].Benefit.Name;
                        requestToApprove1.CanCancel = benefitRequestModels[index].CanCancel;
                        requestToApprove1.CanEdit = benefitRequestModels[index].CanEdit;
                        requestToApprove1.RequestNumber = benefitRequestModels[index].Id;
                        requestToApprove1.From = benefitRequestModels[index].ExpectedDateFrom.ToString("yyyy-MM-dd");
                        requestToApprove1.To = benefitRequestModels[index].ExpectedDateTo.ToString("yyyy-MM-dd");
                        requestToApprove1.Requestedat = benefitRequestModels[index].CreatedDate.ToString("yyyy-MM-dd");
                        requestToApprove1.Message = benefitRequestModels[index].Message;
                        requestToApprove1.status = CommanData.RequestStatusModels.Where(s => s.Id == benefitRequestModels[index].RequestStatusId).First().Name;
                        requestToApprove1.BenefitType = benefitRequestModels[index].Benefit.BenefitType.Name;

                        if (benefitRequestModels[index].WarningMessage != null)
                        {
                            requestToApprove1.WarningMessage = benefitRequestModels[index].WarningMessage;
                        }
                        List<EmployeeModel> employeeModels1 = new List<EmployeeModel>();
                        employeeModels1.Add(benefitRequestModels[index].Employee);
                        requestToApprove1.CreatedBy = CreateEmployeeData(employeeModels1).First();
                        if (benefitRequestModels[index].Group != null)
                        {
                            requestToApprove1.GroupName = benefitRequestModels[index].Group.Name;
                            List<EmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)benefitRequestModels[index].GroupId).Result.Select(eg => eg.Employee).ToList();
                            groupEmployeeModels.Where(ge => ge.EmployeeNumber == benefitRequestModels[index].EmployeeId).First().IsTheGroupCreator = true;
                            if (groupEmployeeModels != null)
                            {
                                List<LoginUser> employeesData = CreateEmployeeData(groupEmployeeModels);
                                requestToApprove1.FullParticipantsData = employeesData;
                            }
                            List<EmployeeModel> employeeModels2 = new List<EmployeeModel>();
                            employeeModels2.Add(_EmployeeService.GetEmployee(employeeNumber));
                            requestToApprove1.CreatedBy = CreateEmployeeData(employeeModels2).First();
                        }
                        if (benefitRequestModels[index].SendTo != 0)
                        {
                            EmployeeModel employeeModel = _EmployeeService.GetEmployee(benefitRequestModels[index].SendTo);
                            List<EmployeeModel> employeeModels = new List<EmployeeModel>();
                            employeeModels.Add(employeeModel);
                            requestToApprove1.SendToModel = CreateEmployeeData(employeeModels).First();
                        }
                        if (requestToApprove1.status != "Cancelled")
                        {
                            List<RequestWorkFlowAPI> requestWorkFlowAPIs = new List<RequestWorkFlowAPI>();
                            requestWorkFlowAPIs = AddWorkflowToRequest(benefitRequestModels[index]);
                            requestToApprove1.RequestWorkFlowAPIs = requestWorkFlowAPIs;
                        }

                        requestsToApprove.Add(requestToApprove1);
                    }
                    return requestsToApprove;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public RequestWorkFlowAPI CreateRequestWorkFlowAPI(RequestWokflowModel requestWokflowModel)
        {
            try
            {
                RequestWorkFlowAPI requestWorkFlowAPI = new RequestWorkFlowAPI();
                requestWorkFlowAPI.EmployeeNumber = requestWokflowModel.EmployeeId;
                requestWorkFlowAPI.EmployeeName = requestWokflowModel.Employee.FullName;
                requestWorkFlowAPI.employeeCanResponse = requestWokflowModel.canResponse;
                requestWorkFlowAPI.Notes = requestWokflowModel.Notes;
                requestWorkFlowAPI.ReplayDate = requestWorkFlowAPI.ReplayDate;
                requestWorkFlowAPI.Status = Enum.GetName(typeof(CommanData.BenefitStatus), requestWokflowModel.RequestStatusId);
                return requestWorkFlowAPI;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public RequestWorkFlowAPI CreateRequestWorkFlowAPIBasic(EmployeeModel employeeModel, string status)
        {
            try
            {

                RequestWorkFlowAPI requestWorkFlowAPI = new RequestWorkFlowAPI();
                requestWorkFlowAPI.EmployeeNumber = employeeModel.EmployeeNumber;
                requestWorkFlowAPI.EmployeeName = employeeModel.FullName;
                requestWorkFlowAPI.Status = status;
                return requestWorkFlowAPI;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public List<RequestWorkFlowAPI> AddWorkflowToRequest(BenefitRequestModel benefitRequestModel)
        {
            List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
            List<RequestWorkFlowAPI> requestWorkFlowAPIs = new List<RequestWorkFlowAPI>();
            EmployeeModel employeeModel = new EmployeeModel();

            int start = 0;
            int index = 0;
            int length = 0;
            int actualWorkflowCount = benefitWorkflowModels.Count;
            int requestWorkflowCount = benefitRequestModel.RequestWokflowModels.Count;

            if (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending ||
                benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.InProgress ||
                (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Approved) ||
                (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Rejected))
            {
                length = benefitRequestModel.RequestWokflowModels.Count;

            }
            else if (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Cancelled)
            {
                length = benefitRequestModel.RequestWokflowModels.Count - 1;
            }
            //else if (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Approved ||
            //    (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Rejected))
            //{
            //    length = benefitRequestModel.RequestWokflowModels.Count;
            //}

            for (index = 0; index < length; index++)
            {
                RequestWorkFlowAPI requestWorkFlowAPI = CreateRequestWorkFlowAPI(benefitRequestModel.RequestWokflowModels[index]);

                requestWorkFlowAPIs.Add(requestWorkFlowAPI);
            }
            start = index;

            for (int index2 = start; index2 < benefitWorkflowModels.Count; index2++)
            {
                string status = "Not Yet";
                string roleName = "";

                roleName = _roleService.GetRole(benefitWorkflowModels[index2].RoleId).Result.Name;
                if (roleName == "HR")
                {
                    employeeModel.EmployeeNumber = 0;
                    employeeModel.FullName = "HR";

                }
                else if (roleName == "Supervisor")
                {
                    employeeModel = _EmployeeService.GetEmployee((long)benefitRequestModel.Employee.SupervisorId);
                }
                else if (roleName == "Department Manager")
                {
                    employeeModel = _EmployeeService.GetDepartmentManager(benefitRequestModel.Employee.DepartmentId);
                }

                RequestWorkFlowAPI requestWorkFlowAPI = CreateRequestWorkFlowAPIBasic(employeeModel, status);
                requestWorkFlowAPIs.Add(requestWorkFlowAPI);
            }
            return requestWorkFlowAPIs;
        }

        public string AddIndividualRequest(Request request, string userId, BenefitModel benefitModel)
        {
            BenefitRequestModel NewBenefitRequestModel = new BenefitRequestModel();
            List<RequestDocumentModel> requestDocumentModels = new List<RequestDocumentModel>();
            EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userId).Result;
            string result = "";
            //if (benefitModel.IsAgift == false && benefitRequestModel.SendTo == 0 || benefitModel.IsAgift == true && benefitRequestModel.SendTo != 0)
            //{
            NewBenefitRequestModel.EmployeeId = employeeModel.EmployeeNumber;
            NewBenefitRequestModel.Message = request.Message;
            NewBenefitRequestModel.CreatedDate = DateTime.Now;
            NewBenefitRequestModel.RequestDate = DateTime.Now;
            NewBenefitRequestModel.UpdatedDate = DateTime.Now;
            NewBenefitRequestModel.IsVisible = true;
            NewBenefitRequestModel.IsDelted = false;
            NewBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
            NewBenefitRequestModel.BenefitId = request.benefitId;
            NewBenefitRequestModel.ExpectedDateFrom = Convert.ToDateTime(request.From);
            if (request.To == null)
            {
                NewBenefitRequestModel.ExpectedDateTo = Convert.ToDateTime(request.From);
            }
            else
            {
                NewBenefitRequestModel.ExpectedDateTo = Convert.ToDateTime(request.To);

            }
            //if (request.Documents != null)
            //{
            //    string filePath = "";
            //    foreach (var file in request.Documents)
            //    {
            //        RequestDocumentModel requestDocumentModel = new RequestDocumentModel();
            //        filePath = UploadedImageAsync(file, @"C:\inetpub\wwwroot\_more4u\wwwroot\BenefitRequestFiles").Result;
            //        requestDocumentModel.fileName = filePath;
            //        requestDocumentModel.FileType = file.ContentType;
            //        requestDocumentModels.Add(requestDocumentModel);
            //    }
            //}
            if (request.SendToId != 0)
            {
                NewBenefitRequestModel.SendTo = request.SendToId;
            }
            BenefitRequestModel addedRequest = _benefitRequestService.CreateBenefitRequest(NewBenefitRequestModel);
            if (addedRequest != null)
            {
                //if (requestDocumentModels != null)
                //{
                //    foreach (var document in requestDocumentModels)
                //    {
                //        document.BenefitRequestId = addedRequest.Id;
                //        _requestDocumentService.CreateRequestDocument(document);
                //    }
                //}

                BenefitRequestModel newBenefitRequestModel = new BenefitRequestModel();
                newBenefitRequestModel.Benefit = benefitModel;
                if (newBenefitRequestModel.Benefit.HasWorkflow)
                {
                    result = SendReuestToWhoIsConcern(addedRequest.Id, 1).Result;
                    if (result.Contains("successful Process"))
                    {
                        result = "Success Process, you added new request for " + benefitModel.Name
                            + "your requested date from " + addedRequest.ExpectedDateFrom.ToString("yyyy-MM-dd")
                            + "To " + addedRequest.ExpectedDateTo.ToString("yyyy-MM-dd");

                    }
                    else
                    {
                        result = "There is problem in workflow";
                    }
                }

            }
            else
            {
                result = "Sorry, your request can't be created";

            }
            //}
            return result;
        }

        public async Task<string> UploadedImageAsync(IFormFile ImageName, string path)
        {
            string uniqueFileName = null;
            string filePath = null;

            if (ImageName != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, path);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageName.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageName.CopyToAsync(fileStream);
                }

                //using (var dataStream = new MemoryStream())
                //{
                //    await ImageName.CopyToAsync(dataStream);
                //}
            }
            return uniqueFileName;
        }

        public async Task<string> SendReuestToWhoIsConcern(long benefitRequetId, int orderNumber)
        {
            try
            {
                string message = "";
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequetId);
                benefitRequestModel.Benefit.BenefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.Benefit.Id);
                if (benefitRequestModel.Benefit.BenefitWorkflowModels != null)
                {
                    BenefitWorkflowModel benefitWorkflowModel = benefitRequestModel.Benefit.BenefitWorkflowModels.Where(w => w.Order == orderNumber).First();
                    string roleName = _roleService.GetRole(benefitWorkflowModel.RoleId).Result.Name;
                    //EmployeeModel employeeWhoRequest = _EmployeeService.GetEmployee(benefitRequestModel.Employee.EmployeeNumber);
                    EmployeeModel whoIsConcern = new EmployeeModel();
                    if (roleName != null)
                    {
                        if (roleName == "Supervisor")
                        {
                            whoIsConcern = _EmployeeService.GetEmployee((long)benefitRequestModel.Employee.SupervisorId);
                        }
                        else if (roleName == "HR")
                        {
                            //EmployeeModel HR = _EmployeeService.GetEmployee((long)benefitRequestModel.Employee.HRId);
                            DepartmentModel departmentModel = _departmentService.GetDepartmentByName("HR");
                            whoIsConcern = _EmployeeService.GetDepartmentManager(departmentModel.Id);
                        }
                        else if (roleName == "Department Manager")
                        {
                            DepartmentModel departmentModel = _departmentService.GetDepartment(benefitRequestModel.Employee.DepartmentId);
                            whoIsConcern = _EmployeeService.GetDepartmentManager(departmentModel.Id);
                        }

                        if (whoIsConcern != null)
                        {

                            RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
                            requestWokflowModel.EmployeeId = whoIsConcern.EmployeeNumber;
                            requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
                            requestWokflowModel.RoleId = benefitWorkflowModel.RoleId;
                            requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                            requestWokflowModel.CreatedDate = DateTime.Now;
                            requestWokflowModel.UpdatedDate = DateTime.Now;
                            requestWokflowModel.IsDelted = false;
                            requestWokflowModel.IsVisible = true;
                            var requestWorkflow = CreateRequestWorkflow(requestWokflowModel);
                            if (requestWorkflow != null)
                            {
                                RequestWokflowModel requestWokflowModel1 = GetRequestWorkflowByEmployeeNumber(requestWorkflow.Result.EmployeeId, requestWorkflow.Result.BenefitRequestId);
                                message = "successful Process, your request will be proceed";
                                //bool result = SendNotification(benefitRequestModel, requestWokflowModel1, "Request").Result;


                                //NotificationModel notificationModel = CreateNotification("Request", requestWokflowModel1);
                                //await SendToSpecificUser(requestWokflowModel1, "Request");
                            }
                            else
                            {
                                message = "Failed Process, failed to send it";
                            }
                        }
                        else
                        {
                            message = "Failed Process, There is a problem in this benefit";
                        }
                    }
                    else
                    {
                        message = "Failed Process, There is a problem in this benefit";
                    }
                }
                else
                {
                    message = "Failed Process, There is a problem in this benefit";
                }
                return message;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return "There is a problem, please contact with your Technical support";
            }
        }


        public async Task<string> ConfirmGroupRequest(Request request, string userId, BenefitModel benefitModel)
        {
            EmployeeModel CurrentEmployee = await _EmployeeService.GetEmployeeByUserId(userId);
            string[] insertedEmployeeNumbersString = request.SelectedEmployeeNumbers.Split(";");
            insertedEmployeeNumbersString[insertedEmployeeNumbersString.Length - 1] = CurrentEmployee.EmployeeNumber.ToString();
            //BenefitModel benefitModel = _BenefitService.GetBenefit(groupModel.BenefitRequestModel.Benefit.Id);
            int GroupMembersCount = insertedEmployeeNumbersString.Length;
            bool result = false;
            string Message = "";
            GroupModel groupModel = new GroupModel();
            if (GroupMembersCount <= benefitModel.MaxParticipant && GroupMembersCount >= benefitModel.MinParticipant)
            {
                groupModel.BenefitId = request.benefitId;
                groupModel.CreatedDate = DateTime.Now;
                groupModel.UpdatedDate = DateTime.Now;
                groupModel.IsDelted = false;
                groupModel.IsVisible = true;
                groupModel.CreatedBy = CurrentEmployee.Id;
                groupModel.ExpectedDateFrom = Convert.ToDateTime(request.From);
                groupModel.Message = request.Message;
                groupModel.Name = request.GroupName;
                if (request.To == null)
                {
                    groupModel.ExpectedDateTo = Convert.ToDateTime(request.From);
                }
                if (GroupMembersCount == benefitModel.MaxParticipant)
                {
                    groupModel.GroupStatus = "Closed";
                    groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                }
                else if (GroupMembersCount >= benefitModel.MinParticipant)
                {
                    groupModel.GroupStatus = "Open";
                    groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                }
                groupModel.Benefit = null;
                groupModel.BenefitRequestModel = null;
                GroupModel newGroupModel = _groupService.CreateGroup(groupModel);
                if (newGroupModel != null)
                {
                    newGroupModel.Code = "G-" + DateTime.Today.ToString("yyyyMMdd") + newGroupModel.Id;
                    result = _groupService.UpdateGroup(newGroupModel);
                    GroupEmployeeModel groupEmployeeModel = new GroupEmployeeModel();
                    GroupEmployeeModel newGroupEmployeeModel = new GroupEmployeeModel();
                    if (result == true)
                    {
                        for (int index = 0; index < insertedEmployeeNumbersString.Length; index++)
                        {
                            long employeeNumber = long.Parse(insertedEmployeeNumbersString[index]);
                            EmployeeModel employeeMember = _EmployeeService.GetEmployee(employeeNumber);
                            groupEmployeeModel.EmployeeId = employeeMember.EmployeeNumber;
                            groupEmployeeModel.GroupId = newGroupModel.Id;
                            groupEmployeeModel.JoinDate = DateTime.Now;
                            newGroupEmployeeModel = _groupEmployeeService.CreateGroupEmployee(groupEmployeeModel);
                        }
                    }
                    BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
                    benefitRequestModel.RequestDate = DateTime.Now;
                    benefitRequestModel.CreatedDate = DateTime.Now;
                    benefitRequestModel.UpdatedDate = DateTime.Now;
                    benefitRequestModel.IsDelted = false;
                    benefitRequestModel.IsVisible = true;
                    benefitRequestModel.Message = groupModel.Message;
                    benefitRequestModel.ExpectedDateFrom = groupModel.ExpectedDateFrom;
                    benefitRequestModel.ExpectedDateTo = groupModel.ExpectedDateTo;
                    benefitRequestModel.BenefitId = benefitModel.Id;
                    benefitRequestModel.GroupId = newGroupModel.Id;
                    benefitRequestModel.EmployeeId = CurrentEmployee.EmployeeNumber;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                    BenefitRequestModel newBenefitRequestModel = _benefitRequestService.CreateBenefitRequest(benefitRequestModel);
                    if (newBenefitRequestModel != null)
                    {
                        result = SendGroupRequestToHR(newBenefitRequestModel);
                        if (result == true)
                        {
                            Message = "Success Process, your request has been sent";

                        }
                        else
                        {
                            Message = "Problem in Workflow";
                        }

                    }
                    else
                    {
                        Message = "Can not send Request";
                    }
                }
                else
                {
                    Message = "Can not create group";
                }
            }
            else
            {
                Message = "Failed Process, Group Members does not match";
            }


            return Message;
        }

        public bool SendGroupRequestToHR(BenefitRequestModel benefitRequestModel)
        {
            try
            {
                List<AspNetUser> HRUsers = _userManager.GetUsersInRoleAsync("HR").Result.ToList();
                RoleModel roleModel = _roleService.GetRoleByName("HR").Result;
                RequestWokflowModel newRequestWokflowModel = new RequestWokflowModel();
                bool result = false;
                foreach (AspNetUser user in HRUsers)
                {
                    EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(user.Id).Result;
                    RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
                    requestWokflowModel.EmployeeId = employeeModel.EmployeeNumber;
                    requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
                    requestWokflowModel.RoleId = roleModel.Id;
                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                    requestWokflowModel.CreatedDate = DateTime.Today;
                    requestWokflowModel.UpdatedDate = DateTime.Today;
                    requestWokflowModel.IsDelted = false;
                    requestWokflowModel.IsVisible = true;
                    newRequestWokflowModel = CreateRequestWorkflow(requestWokflowModel).Result;
                    benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                    if (newRequestWokflowModel != null)
                    {
                        newRequestWokflowModel = GetRequestWorkflowByEmployeeNumber(newRequestWokflowModel.EmployeeId, newRequestWokflowModel.BenefitRequestId);
                        result = SendNotification(benefitRequestModel, newRequestWokflowModel, "Request").Result;

                    }
                }
                result = SendNotification(benefitRequestModel, newRequestWokflowModel, "CreateGroup").Result;

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        public async Task<bool> SendNotification(BenefitRequestModel benefitRequestModel, RequestWokflowModel DBRequestWorkflowModel, string type)
        {
            try
            {
                NotificationModel notificationModel = new NotificationModel();
                string notificationMessage = "";
                if (type == "CreateGroup")
                {
                    List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)DBRequestWorkflowModel.BenefitRequest.GroupId).Result.ToList();
                    notificationMessage = benefitRequestModel.Employee.FullName + " added you to new group for " + benefitRequestModel.Benefit.Name + " benefit";
                    await SendNotificationToGroupMembers(groupEmployeeModels, DBRequestWorkflowModel, benefitRequestModel, notificationMessage, type);
                }
                else
                {
                    DBRequestWorkflowModel.BenefitRequest = benefitRequestModel;

                    //if (benefitRequestModel.Benefit.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
                    //{
                    if (type == "Request")
                    {
                        notificationMessage = benefitRequestModel.Employee.FullName + " added new request for " + benefitRequestModel.Benefit.Name + " benefit";
                        notificationModel = CreateNotification(type, DBRequestWorkflowModel.EmployeeId, benefitRequestModel.Id, notificationMessage, 0);
                        await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, benefitRequestModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                        // var token = "";
                        // await _firebaseNotificationService.SendNotification("Request", notificationMessage, token);
                    }
                    if (type == "RequestCancel")
                    {
                        notificationMessage = benefitRequestModel.Employee.FullName + " cancelled his request for " + benefitRequestModel.Benefit.Name + " benefit";
                        notificationModel = CreateNotification(type, DBRequestWorkflowModel.EmployeeId, benefitRequestModel.Id, notificationMessage, 0);
                        await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, benefitRequestModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);

                    }
                    else if (type == "Response")
                    {
                        if (DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Approved)
                        {
                            notificationMessage = DBRequestWorkflowModel.Employee.FullName + " Approved your request for " + benefitRequestModel.Benefit.Name + " benefit";
                            if (benefitRequestModel.SendTo != 0)
                            {
                                EmployeeModel employee = _EmployeeService.GetEmployee(benefitRequestModel.SendTo);
                                RequestWokflowModel newRequestWokflowModel = DBRequestWorkflowModel;
                                newRequestWokflowModel.BenefitRequest.Employee = employee;
                                notificationMessage = benefitRequestModel.Employee.FullName + "send a new gift to you from" + benefitRequestModel.Benefit.Name + "benefit";
                                notificationModel = CreateNotification(type, newRequestWokflowModel.BenefitRequest.SendTo, benefitRequestModel.Id, notificationMessage, 0);
                                await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, benefitRequestModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                            }
                        }
                        else
                        {
                            notificationMessage = DBRequestWorkflowModel.Employee.FullName + " Rejected your request for " + benefitRequestModel.Benefit.Name + " benefit";
                        }
                        if (benefitRequestModel.GroupId == null)
                        {
                            notificationModel = CreateNotification(type, benefitRequestModel.EmployeeId, benefitRequestModel.Id, notificationMessage, DBRequestWorkflowModel.EmployeeId);
                            await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, DBRequestWorkflowModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                        }
                        else
                        {
                            List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)benefitRequestModel.GroupId).Result.ToList();

                            await SendNotificationToGroupMembers(groupEmployeeModels, DBRequestWorkflowModel, benefitRequestModel, notificationMessage, type);

                        }
                    }


                    // }
                    //else if (benefitRequestModel.Benefit.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
                    //{


                    //}
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }


        public async Task<bool> SendNotificationToGroupMembers(List<GroupEmployeeModel> groupEmployeeModels, RequestWokflowModel DBRequestWorkflowModel, BenefitRequestModel benefitRequestModel, string notificationMessage, string type)
        {
            try
            {
                NotificationModel notificationModel = null;
                string userId = benefitRequestModel.Employee.UserId;
                foreach (GroupEmployeeModel groupEmployeeModel in groupEmployeeModels)
                {
                    RequestWokflowModel newRequestWokflowModel = DBRequestWorkflowModel;
                    newRequestWokflowModel.BenefitRequest.Employee = groupEmployeeModel.Employee;
                    notificationModel = CreateNotification(type, groupEmployeeModel.Employee.EmployeeNumber, benefitRequestModel.Id, notificationMessage, 0);
                    await SendToSpecificUser(notificationMessage, newRequestWokflowModel, type, benefitRequestModel.Employee.FullName, userId);
                }
                if (notificationModel != null)
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

        public NotificationModel CreateNotification(string Type, long employeeNumber, long BenefitRequestId, string message, long responsedBy)
        {
            NotificationModel notificationModel = new NotificationModel();
            notificationModel.IsDelted = false;
            notificationModel.IsVisible = true;
            notificationModel.UpdatedDate = DateTime.Now;
            notificationModel.CreatedDate = DateTime.Now;
            notificationModel.BenefitRequestId = BenefitRequestId;
            notificationModel.Type = Type;
            notificationModel.Message = message;
            if (Type == "Response")
            {
                notificationModel.ResponsedBy = responsedBy;
            }
            NotificationModel newNotificationModel = _notificationService.CreateNotification(notificationModel);

            if (newNotificationModel != null)
            {
                UserNotificationModel userNotificationModel = new UserNotificationModel();
                userNotificationModel.CreatedDate = DateTime.Now;
                userNotificationModel.UpdatedDate = DateTime.Now;
                userNotificationModel.EmployeeId = employeeNumber;

                //if (Type == "Response")
                //{
                //    userNotificationModel.EmployeeId = employeeNumber;
                //}
                //else if (Type == "Request")
                //{
                //    userNotificationModel.EmployeeId = employeeNumber;
                //}
                //else if(Type =="CreateGroup")
                //{
                //    userNotificationModel.EmployeeId = employeeNumber;
                //}
                userNotificationModel.NotificationId = newNotificationModel.Id;
                userNotificationModel.Seen = false;
                UserNotificationModel newUserotificationModel = _userNotificationService.CreateUserNotification(userNotificationModel);
            }
            return newNotificationModel;
        }


        public async Task<bool> SendToSpecificUser(string message, RequestWokflowModel model, string requestType, string employeeName, string userId)
        {
            var connections = _userConnectionManager.GetUserConnections(model.Employee.UserId);
            if (requestType == "Request" || requestType == "RequestCancel")
            {
                connections = _userConnectionManager.GetUserConnections(model.Employee.UserId);
            }
            else
            {
                connections = _userConnectionManager.GetUserConnections(model.BenefitRequest.Employee.UserId);
            }
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    await _hub.Clients.Client(connectionId).SendAsync("sendToUser", requestType, model.CreatedDate.Date.ToString("dd-MM-yyyy"), model.CreatedDate.ToShortTimeString(), model.BenefitRequest.Benefit.Id, message, employeeName, userId);
                    //if (requestType == "Request")
                    //{
                    //    await _hub.Clients.Client(connectionId).SendAsync("sendToUser", requestType, model.CreatedDate.Date.ToString("dd-MM-yyyy"), model.CreatedDate.ToShortTimeString(), model.BenefitRequest.Benefit.Id, message, employeeName, userId);
                    //}
                    //else if(requestType == "Response")
                    //{
                    //    await _hub.Clients.Client(connectionId).SendAsync("sendToUser", requestType, model.CreatedDate.Date.ToString("dd-MM-yyyy"), model.CreatedDate.ToShortTimeString(), model.BenefitRequest.Benefit.Id, message, employeeName, userId);
                    //}
                    //else if(requestType == "CreateGroup")
                    //{
                    //    await _hub.Clients.Client(connectionId).SendAsync("sendToUser", requestType, model.CreatedDate.Date.ToString("dd-MM-yyyy"), model.CreatedDate.ToShortTimeString(), model.BenefitRequest.Benefit.Id, message, employeeName, userId);
                    //}
                }
            }
            return true;
        }

        public Task<string> AddDocumentsToRequest(long requestNumber, List<IFormFile> files)
        {
            string message = "";
            if (files.Count != 0)
            {

                string filePath = "";
                int count = 0;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        RequestDocumentModel requestDocumentModel = new RequestDocumentModel();
                        filePath = UploadedImageAsync(file, "BenefitRequestFiles").Result;
                        requestDocumentModel.fileName = filePath;
                        requestDocumentModel.FileType = file.ContentType;
                        requestDocumentModel.BenefitRequestId = requestNumber;
                        RequestDocumentModel requestDocument = _requestDocumentService.CreateRequestDocument(requestDocumentModel);
                        if (requestDocument != null)
                        {
                            count++;
                        }
                    }

                }
                if (count == files.Count)
                {
                    message = "Success Process, you upload " + count + "files";
                }
                else
                {
                    message = "failed Process";
                }

            }
            return Task<string>.FromResult(message);
        }

    }
}
