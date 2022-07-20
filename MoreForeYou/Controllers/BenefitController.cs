using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

using static MoreForYou.Services.Models.CommanData;
using MoreForYou.Controllers.hub;
using Microsoft.AspNetCore.SignalR;
using MoreForYou.Services.Models.API;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MoreForYou.Controllers
{
    public class BenefitController : Controller
    {
        private readonly IBenefitService _BenefitService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<BenefitController> _logger;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _EmployeeService;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IRequestWorkflowService _requestWorkflowService;
        private readonly IDepartmentService _departmentService;
        private readonly IBenefitTypeService _benefitTypeService;
        private readonly IRequestStatusService _requestStatusService;
        //private readonly IEmployeeRequestService _employeeRequestService;
        private readonly IGroupService _groupService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IRequestDocumentService _requestDocumentService;
        private readonly IFirebaseNotificationService _firebaseNotificationService;

        public BenefitController(IBenefitService BenefitService,
            IBenefitWorkflowService BenefitWorkflowService,
            IUserService userService,
            IRoleService roleService,
            ILogger<BenefitController> logger,
            UserManager<AspNetUser> userManager,
            IEmployeeService EmployeeService,
            IBenefitRequestService benefitRequestService,
            IRequestWorkflowService requestWorkflowService,
            IDepartmentService departmentService,
            IBenefitTypeService benefitTypeService,
            IRequestStatusService requestStatusService,
            IGroupService groupService,
            IWebHostEnvironment hostEnvironment,
            IGroupEmployeeService groupEmployeeService,
            IHubContext<NotificationHub> hub,
            INotificationService notificationService,
            IUserNotificationService userNotificationService,
            IUserConnectionManager userConnectionManager,
            IRequestDocumentService requestDocumentService,
            IFirebaseNotificationService firebaseNotificationService)
        {
            _BenefitService = BenefitService;
            _benefitWorkflowService = BenefitWorkflowService;
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
            _userManager = userManager;
            _EmployeeService = EmployeeService;
            _benefitRequestService = benefitRequestService;
            _requestWorkflowService = requestWorkflowService;
            _departmentService = departmentService;
            _benefitTypeService = benefitTypeService;
            _requestStatusService = requestStatusService;
            _groupService = groupService;
            _hostEnvironment = hostEnvironment;
            _groupEmployeeService = groupEmployeeService;
            _hub = hub;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _userConnectionManager = userConnectionManager;
            _requestDocumentService = requestDocumentService;
            _firebaseNotificationService = firebaseNotificationService;
        }

        List<GenderModel> genderList = new List<GenderModel>()
            {
                new GenderModel {  Name='M'},
                new GenderModel {  Name='F'}
            };

        List<ResonseStatus> resonseStatuses = new List<ResonseStatus>()
        {
            new ResonseStatus {Id =-1, Name ="None"},
            new ResonseStatus {Id =1, Name ="Approve"},
            new ResonseStatus {Id =1, Name ="Disapprove"},
        };
        List<RequestStatusModel> whoIsConcernRequestStatusModels = new List<RequestStatusModel>()
        {
            new RequestStatusModel {Id =-1, Name ="Status"},
            new RequestStatusModel {Id =1, Name ="Pending"},
            new RequestStatusModel {Id =3, Name ="Approved"},
            new RequestStatusModel {Id =4, Name ="Rejected"},

        };

        List<TimingModel> timingModels = new List<TimingModel>()
        {
             new TimingModel{Id=-1, Name="Date"},
             new TimingModel{Id=1, Name="Today"},
            new TimingModel{Id=2, Name="Last Day"},
            new TimingModel{Id=3, Name="Current Week"},
            new TimingModel{Id=4, Name="Current Month"},
        };

        public List<Collar> Collars = new List<Collar>()
        {
               new Collar { Id = -1, Name = "Any" },
            new Collar { Id = 1, Name = "White Collar" },
            new Collar { Id = 2, Name = "Blue Collar" }

        };

        public List<string> AgeSigns = new List<string>()
        {
           ">",
           "<",
           "="
        };

        public List<string> DatesToMatch = new List<string>()
        {
            "Any",
           "Birth Date",
           "Join Date",
           "certain Date"
        };

        public JsonResult supervisorFilter(long id)
        {
            try
            {
                EmployeeModel Supervisor = new EmployeeModel();
                if (id == 0)
                {
                    Supervisor = null;
                }
                else
                {
                    Supervisor = _EmployeeService.GetEmployee(id);
                }
                if (Supervisor != null)
                {
                    return Json(Supervisor.FullName);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                //return (JsonResult)ERROR404();
            }
            return null;
        }

        // GET: BenefitController
        public ActionResult Index()
        {
            List<BenefitModel> beniefitModels = _BenefitService.GetAllBenefits().Result;
            foreach (BenefitModel benefitModel in beniefitModels)
            {
                if (benefitModel.gender == 0)
                {
                    benefitModel.GenderText = "Any";
                }
                else if (benefitModel.gender == 1)
                {
                    benefitModel.GenderText = "Male";
                }
                else if (benefitModel.gender == 2)
                {
                    benefitModel.GenderText = "Female";
                }


                if (benefitModel.MaritalStatus == 0)
                {
                    benefitModel.MartialStatusText = "Any";
                }
                else if (benefitModel.MaritalStatus == 1)
                {
                    benefitModel.MartialStatusText = "Single";
                }
                else if (benefitModel.MaritalStatus == 2)
                {
                    benefitModel.MartialStatusText = "Married";
                }
                else if (benefitModel.MaritalStatus == 3)
                {
                    benefitModel.MartialStatusText = "Divorced";
                }


            }

            return View(beniefitModels);
        }

        // GET: BenefitController/Details/5
        //public ActionResult BenefitDetails(long id)
        //{
        //    BenefitModel benefitModel = _BenefitService.GetBenefit(id);
        //    benefitModel.BenefitConditions = CreateBenefitConditions(benefitModel);
        //    return View(benefitModel);
        //}

        public async Task<ActionResult> RequestDetails(long id)
        {
            AspNetUser applicationUser = await _userManager.GetUserAsync(User);
            EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(applicationUser.Id).Result;
            BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
            List<RequestWokflowModel> requestWokflowModels = _requestWorkflowService.GetRequestWorkflow(id).Where(rw => rw.EmployeeId == employeeModel.EmployeeNumber).ToList();
            return View(benefitRequestModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestCancel(long id, long benefitId)
        {
            string message = _requestWorkflowService.CancelMyRequest(id);
            if (message == "Success Process")
            {
                TempData["Message"] = "Sucess process, your request with number " + id + " has been cancelled";
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
                RequestWokflowModel requestWokflowModel = _requestWorkflowService.GetRequestWorkflow(id).First();
                bool result = SendNotification(benefitRequestModel, requestWokflowModel, "RequestCancel").Result;

            }
            else
            {
                TempData["Error"] = message;
            }
            return RedirectToAction("ShowMyBenefitRequests", new { BenefitId = benefitId });
        }

        // GET: BenefitController/Create
        public ActionResult Create()
        {
            BenefitModel benefitModel = new BenefitModel();
            List<RoleModel> RoleModels = _roleService.GetAllRoles().Result;
            benefitModel.Collars = Collars;
            benefitModel.DatesToMatch = DatesToMatch;
            benefitModel.RolesOrder = new List<RoleOrder>();
            benefitModel.genderModels = genderList;
            benefitModel.AgeSigns = AgeSigns;
            for (int index = 0; index < RoleModels.Count; index++)
            {
                RoleOrder roleOrder = new RoleOrder()
                { order = 0, RoleId = RoleModels[index].Id, RoleName = RoleModels[index].Name };
                benefitModel.RolesOrder.Insert(index, roleOrder);
            }
            benefitModel.RolesOrder = benefitModel.RolesOrder.Where(r => r.RoleName != "Admin").ToList();
            return View(benefitModel);
        }

        // POST: BenefitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BenefitModel Model)
        {
            try
            {
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                Model.HasWorkflow = true;
                Model.BenefitReturn = 1;
                string imageName = _requestWorkflowService.UploadedImageAsync(Model.ImageName, @"http://20.86.97.165/more4u/_more4u/wwwroot/images/BenefitCards").Result;
                Model.BenefitCard = imageName;
                var addedBenefitModel = _BenefitService.CreateBenefit(Model);
                if (addedBenefitModel != null)
                {
                    if (addedBenefitModel.HasWorkflow == true)
                    {

                        bool response = false;
                        foreach (var workflow in Model.RolesOrder.Where(R => R.order != 0))
                        {
                            BenefitWorkflowModel benefitWorkflowModel = new BenefitWorkflowModel();
                            benefitWorkflowModel.RoleId = workflow.RoleId;
                            benefitWorkflowModel.BenefitId = addedBenefitModel.Id;
                            benefitWorkflowModel.Order = workflow.order;
                            benefitWorkflowModel.IsDelted = false;
                            benefitWorkflowModel.IsVisible = true;
                            benefitWorkflowModel.CreatedDate = DateTime.Today;
                            benefitWorkflowModel.UpdatedDate = DateTime.Today;
                            response = _benefitWorkflowService.CreateBenefitWorkflow(benefitWorkflowModel).Result;
                            if (response != true)
                            {
                                response = _benefitWorkflowService.DeleteBenefitWorkflow(addedBenefitModel.Id);
                                ViewBag.Error = "Failed process, Fail to add Benefit workflow";
                                break;
                            }
                        }
                        if (response == true)
                        {
                            ViewBag.Message = "Success Process, Benefit has been added";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Success Process, Benefit has been added";
                    }
                }
                else
                {
                    ViewBag.Error = "Failed to add new Benefit";
                }
                return View("Index");
            }
            catch (Exception e)
            {
                return View("Index");
            }
        }

        //private  List<RequestDocumentModel> UploadedFileAsync(IFormFile[] files, string path)
        //{
        //    string uniqueFileName = null;
        //    string filePath = null;
        //    List<RequestDocumentModel> RequestDocumentModels = new List<RequestDocumentModel>();
        //    for(int x =0; x<files.Length; x++)
        //    {
        //        RequestDocumentModel requestDocumentModel = new RequestDocumentModel();
        //        if (files[x] != null)
        //        {
        //            if (files[x].Length > 0)
        //            {
        //                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, path);
        //                uniqueFileName = Guid.NewGuid().ToString() + "_" + files[x].FileName;
        //                requestDocumentModel.filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //                using (var fileStream = new FileStream(requestDocumentModel.filePath, FileMode.Create))
        //                {
        //                    files[x].CopyTo(fileStream);

        //                }
        //                using (var memoryStrem = new MemoryStream())
        //                {
        //                    files[x].CopyTo(memoryStrem);
        //                    requestDocumentModel.DataFiles = memoryStrem.ToArray();

        //                }
        //                requestDocumentModel.FileType = files[x].ContentType;
        //                requestDocumentModel.Name = files[x].Name;
        //                RequestDocumentModels.Add(requestDocumentModel);
        //            }
        //        }
        //    }

        //    return RequestDocumentModels;
        //}


        // GET: BenefitController/Edit/5
        public ActionResult RequestEdit(int id)
        {
            BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
            return View(benefitRequestModel);
        }

        // POST: BenefitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestEdit(int id, BenefitRequestModel benefitRequestModel)
        {
            try
            {
                BenefitRequestModel DBbenefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                if (DBbenefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)

                {
                    DBbenefitRequestModel.UpdatedDate = DateTime.Today;
                    DBbenefitRequestModel.ExpectedDateFrom = benefitRequestModel.ExpectedDateFrom;
                    DBbenefitRequestModel.ExpectedDateTo = benefitRequestModel.ExpectedDateTo;
                    DBbenefitRequestModel.Message = benefitRequestModel.Message;
                    bool updateResult = _benefitRequestService.UpdateBenefitRequest(DBbenefitRequestModel).Result;
                    if (updateResult == true)
                    {
                        ViewBag.Message = "Sucessful process, you have updated resquest with number " + DBbenefitRequestModel.Id;
                        return View(DBbenefitRequestModel);

                    }
                    else
                    {
                        ViewBag.Error("Failed process");
                        BenefitRequestModel benefitRequestModel1 = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                        return View(benefitRequestModel1);
                    }
                }
                else
                {
                    ViewBag.Error("You can not update this request, As it's status is " + (CommanData.BenefitStatus)DBbenefitRequestModel.RequestStatusId);
                    return View(benefitRequestModel);

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return View(benefitRequestModel);
            }
        }



        // POST: BenefitController/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddResponseAsync(RequestFilterModel RequestFilterModel)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                foreach (RequestWokflowModel requestWokflowModel in RequestFilterModel.RequestWokflowModels)
                {
                    if (requestWokflowModel.canResponse && requestWokflowModel.RequestStatusSelectedId != -1)
                    {
                        RequestWokflowModel DBRequestWorkflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(requestWokflowModel.EmployeeId, requestWokflowModel.BenefitRequestId);
                        requestWokflowModel.ReplayDate = DateTime.Now;
                        requestWokflowModel.RoleId = DBRequestWorkflowModel.RoleId;
                        requestWokflowModel.IsVisible = true;
                        requestWokflowModel.IsDelted = false;
                        requestWokflowModel.UpdatedDate = DateTime.Now;
                        bool updateResult = false;
                        BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(requestWokflowModel.BenefitRequestId);
                        benefitRequestModel.ConfirmedDateFrom = benefitRequestModel.ExpectedDateFrom;
                        benefitRequestModel.ConfirmedDateTo = benefitRequestModel.ExpectedDateTo;
                        if (requestWokflowModel.RequestStatusSelectedId == 2)
                        {
                            requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                            updateResult = _requestWorkflowService.UpdateRequestWorkflow(requestWokflowModel).Result;
                            if (updateResult == true)
                            {
                                updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                                if (updateResult == true)
                                {
                                    ViewBag.Message = "Thank you for kind response";
                                }
                            }
                            else
                            {
                                ViewBag.Error = "Failed process";
                            }
                        }
                        else if (requestWokflowModel.RequestStatusSelectedId == 1)
                        {
                            requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                            updateResult = _requestWorkflowService.UpdateRequestWorkflow(requestWokflowModel).Result;
                            if (updateResult == true)
                            {
                                List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
                                int benefitWorflowsCount = benefitWorkflowModels.Count;
                                int order = benefitWorkflowModels.Where(bw => bw.RoleId == DBRequestWorkflowModel.RoleId).Select(bw => bw.Order).First();
                                if (order == benefitWorflowsCount)
                                {
                                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                                    updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                                    if (updateResult == true)
                                    {
                                        ViewBag.Message = "Thank you for kind response";
                                    }

                                }
                                else if (order <= benefitWorflowsCount)
                                {
                                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.InProgress;
                                    updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                                    if (updateResult == true)
                                    {
                                        int nextOrder = order + 1;
                                        string message = SendReuestToWhoIsConcern(benefitRequestModel.Id, nextOrder).Result;
                                        ViewBag.Message = "Thank you for kind response";
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<ActionResult> ShowBenefits()
        {
            AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
            EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
            HomeModel homeModel = _BenefitService.ShowAllBenefits(employeeModel);
            homeModel.user.Email = CurrentUser.Email;
            return View(homeModel);
        }

        public async Task<ActionResult> BenefitDetails(long id)
        {
            AspNetUser aspNetUser = await _userManager.GetUserAsync(User);
            EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(aspNetUser.Id).Result;
            BenefitAPIModel benefitAPIModel = _BenefitService.GetBenefitDetails(id, employeeModel);
            return View("BenefitDetails", benefitAPIModel);
        }

        // POST: BenefitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowBenefits(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Redeem(int id)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                Request request = _BenefitService.BenefitRedeem(id, CurrentUser.Id);
                if (request.BenefitType == Enum.GetName(typeof(CommanData.BenefitTypes), 2))
                {
                    return View("BenefitRequest", request);
                }
                else
                {
                    return View("GroupRequest", request);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmRequest(Request request)
        {
            try
            {
                string result = "";
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                BenefitModel benefitModel = _BenefitService.GetBenefit(request.benefitId);
                if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
                {
                    result = _requestWorkflowService.AddIndividualRequest(request, CurrentUser.Id, benefitModel);

                }
                else
                {
                    result = _requestWorkflowService.ConfirmGroupRequest(request, CurrentUser.Id, benefitModel).Result;
                }
                if (result.Contains("Success Process"))
                {
                    return RedirectToAction("ShowMyBenefitRequests", new { BenefitId = request.benefitId });
                }
                else
                {
                    ViewBag.Error = result;
                    return View("BenefitRequest", request);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ShowBenefits");
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ConfirmGroupRequest(GroupModel groupModel)
        //{
        //    AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
        //    EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
        //    string[] insertedEmployeeNumbersString = groupModel.BenefitRequestModel.SelectedEmployeeNumbers.Split(";");
        //    insertedEmployeeNumbersString[insertedEmployeeNumbersString.Length - 1] = CurrentEmployee.EmployeeNumber.ToString();
        //    BenefitModel benefitModel = _BenefitService.GetBenefit(groupModel.BenefitRequestModel.Benefit.Id);
        //    int GroupMembersCount = insertedEmployeeNumbersString.Length;
        //    bool result = false;
        //    string Message = "";
        //    if (GroupMembersCount <= benefitModel.MaxParticipant && GroupMembersCount >= benefitModel.MinParticipant)
        //    {
        //        groupModel.BenefitId = groupModel.BenefitRequestModel.Benefit.Id;
        //        groupModel.CreatedDate = DateTime.Now;
        //        groupModel.UpdatedDate = DateTime.Now;
        //        groupModel.IsDelted = false;
        //        groupModel.IsVisible = true;
        //        groupModel.CreatedBy = CurrentUser.Id;
        //        if (groupModel.ExpectedDateTo == null)
        //        {
        //            groupModel.ExpectedDateTo = groupModel.ExpectedDateFrom;
        //        }
        //        if (GroupMembersCount == benefitModel.MaxParticipant)
        //        {
        //            groupModel.GroupStatus = "Closed";
        //            groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //        }
        //        else if (GroupMembersCount >= benefitModel.MinParticipant)
        //        {
        //            groupModel.GroupStatus = "Open";
        //            groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //        }
        //        groupModel.Benefit = null;
        //        groupModel.BenefitRequestModel = null;
        //        GroupModel newGroupModel = _groupService.CreateGroup(groupModel);
        //        if (newGroupModel != null)
        //        {
        //            newGroupModel.Code = "G-" + DateTime.Today.ToString("yyyyMMdd") + newGroupModel.Id;
        //            result = _groupService.UpdateGroup(newGroupModel);
        //            GroupEmployeeModel groupEmployeeModel = new GroupEmployeeModel();
        //            GroupEmployeeModel newGroupEmployeeModel = new GroupEmployeeModel();
        //            if (result == true)
        //            {
        //                for (int index = 0; index < insertedEmployeeNumbersString.Length; index++)
        //                {
        //                    long employeeNumber = long.Parse(insertedEmployeeNumbersString[index]);
        //                    EmployeeModel employeeMember = _EmployeeService.GetEmployee(employeeNumber);
        //                    groupEmployeeModel.EmployeeId = employeeMember.EmployeeNumber;
        //                    groupEmployeeModel.GroupId = newGroupModel.Id;
        //                    groupEmployeeModel.JoinDate = DateTime.Now;
        //                    newGroupEmployeeModel = _groupEmployeeService.CreateGroupEmployee(groupEmployeeModel);
        //                }
        //            }
        //            BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
        //            benefitRequestModel.RequestDate = DateTime.Now;
        //            benefitRequestModel.CreatedDate = DateTime.Now;
        //            benefitRequestModel.UpdatedDate = DateTime.Now;
        //            benefitRequestModel.IsDelted = false;
        //            benefitRequestModel.IsVisible = true;
        //            benefitRequestModel.Message = groupModel.Message;
        //            benefitRequestModel.ExpectedDateFrom = groupModel.ExpectedDateFrom;
        //            benefitRequestModel.ExpectedDateTo = groupModel.ExpectedDateTo;
        //            benefitRequestModel.BenefitId = benefitModel.Id;
        //            benefitRequestModel.GroupId = newGroupModel.Id;
        //            benefitRequestModel.EmployeeId = CurrentEmployee.EmployeeNumber;
        //            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //            BenefitRequestModel newBenefitRequestModel = _benefitRequestService.CreateBenefitRequest(benefitRequestModel);
        //            if (newBenefitRequestModel != null)
        //            {   
        //                result = SendGroupRequestToHR(newBenefitRequestModel);
        //                ViewBag.Message = "Success Process, your request has been sent";
        //                return RedirectToAction("ShowMyBenefits");

        //            }
        //            else
        //            {
        //                ViewBag.Error = "Can not send Request";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Error = "Can not create group";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Failed Process, Group Members does not match";
        //    }

        //    return View(groupModel);
        //}

        //public bool SendGroupRequestToHR(BenefitRequestModel benefitRequestModel)
        //{
        //    try
        //    {
        //        List<AspNetUser> HRUsers = _userManager.GetUsersInRoleAsync("HR").Result.ToList();
        //        RoleModel roleModel = _roleService.GetRoleByName("HR").Result;
        //        RequestWokflowModel newRequestWokflowModel = new RequestWokflowModel();
        //        bool result = false;
        //        foreach (AspNetUser user in HRUsers)
        //        {
        //            EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(user.Id).Result;
        //            RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
        //            requestWokflowModel.EmployeeId = employeeModel.EmployeeNumber;
        //            requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
        //            requestWokflowModel.RoleId = roleModel.Id;
        //            requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //            requestWokflowModel.CreatedDate = DateTime.Today;
        //            requestWokflowModel.UpdatedDate = DateTime.Today;
        //            requestWokflowModel.IsDelted = false;
        //            requestWokflowModel.IsVisible = true;
        //            newRequestWokflowModel = _requestWorkflowService.CreateRequestWorkflow(requestWokflowModel).Result;
        //            benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
        //            if(newRequestWokflowModel != null)
        //            {
        //                newRequestWokflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(newRequestWokflowModel.EmployeeId, newRequestWokflowModel.BenefitRequestId);
        //                result = SendNotification(benefitRequestModel, newRequestWokflowModel, "Request").Result;

        //            }
        //        }
        //        result = SendNotification(benefitRequestModel, newRequestWokflowModel, "CreateGroup").Result;

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //        return false;
        //    }
        //}
        public int CreateGroupMembersRequests(string[] insertedEmployeeNumbersString, GroupModel newGroupModel)
        {
            try
            {
                BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
                benefitRequestModel.RequestDate = DateTime.Now;

                benefitRequestModel.Message = newGroupModel.Message;
                benefitRequestModel.ExpectedDateFrom = newGroupModel.ExpectedDateFrom;
                benefitRequestModel.ExpectedDateTo = newGroupModel.ExpectedDateTo;
                benefitRequestModel.BenefitId = newGroupModel.BenefitId;
                benefitRequestModel.GroupId = newGroupModel.Id;
                benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                BenefitRequestModel newBenefitRequestModel = _benefitRequestService.CreateBenefitRequest(benefitRequestModel);
                if (newBenefitRequestModel != null)
                {
                    RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
                }
                if (newGroupModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)
                {
                    for (int index = 0; index < insertedEmployeeNumbersString.Length; index++)
                    {
                        long employeeNumber = long.Parse(insertedEmployeeNumbersString[index]);
                        EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber);
                    }
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return 0;
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
                            var requestWorkflow = _requestWorkflowService.CreateRequestWorkflow(requestWokflowModel);
                            if (requestWorkflow != null)
                            {
                                RequestWokflowModel requestWokflowModel1 = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(requestWorkflow.Result.EmployeeId, requestWorkflow.Result.BenefitRequestId);
                                message = "successful Process, your request will be proceed";
                                bool result = SendNotification(benefitRequestModel, requestWokflowModel1, "Request").Result;


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

        public async Task<ActionResult> ShowMyBenefits()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<BenefitAPIModel> benefitModels = _BenefitService.GetMyBenefits(CurrentEmployee.EmployeeNumber).ToList();

                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }

                if (benefitModels != null)
                {
                    return View(benefitModels);
                }
                else
                {
                    ViewBag.Error = "you do not have any benefitss";
                    benefitModels = new List<BenefitAPIModel>();
                    return View(benefitModels);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<ActionResult> ShowMyBenefitRequests(long BenefitId)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                long benefitTypeId = _BenefitService.GetBenefit(BenefitId).BenefitTypeId;
                List<Request> requests = _requestWorkflowService.GetMyBenefitRequests(CurrentEmployee.EmployeeNumber, BenefitId, benefitTypeId).ToList();
                MyRequests myRequests = new MyRequests();
                if (requests == null)
                {
                    ViewBag.Error = "Error in the system";
                }
                else
                {
                    myRequests.Requests = requests.OrderByDescending(r => r.Requestedat).ToList();

                }

                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                else if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                return View(myRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }
        public async Task<ActionResult> ShowMyGroups(long BenefitId)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupsByEmployeeId(CurrentEmployee.EmployeeNumber).Result;
                List<GroupModel> groupModels = groupEmployeeModels.Where(g => g.Group.BenefitId == BenefitId).Select(g => g.Group).ToList();
                foreach (GroupModel group in groupModels)
                {
                    group.BenefitRequestModel = _benefitRequestService.GetBenefitRequestByGroupId(group.Id).Result;
                    group.BenefitRequestModel.requestWokflowModel = _requestWorkflowService.GetRequestWorkflow(group.BenefitRequestModel.Id).First();

                }
                return View(groupModels);

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<ActionResult> ShowRequests()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                ManageRequest manageRequest = new ManageRequest();
                if (userRoles != null)
                {
                    if (userRoles.Contains("Admin"))
                    {
                        requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                    }
                    else if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR"))
                    {
                        requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber).Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                    }
                    manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;
                    if (requestWokflowModels.Count != 0)
                    {
                        foreach (var request in requestWokflowModels)
                        {
                            if (request.BenefitRequest.Benefit.RequiredDocuments != null)
                            {
                                List<RequestDocumentModel> requestDocumentModels = _requestDocumentService.GetRequestDocuments(request.BenefitRequestId);
                                if (requestDocumentModels != null)
                                {
                                    //string[] documents = new string[requestDocumentModels.Count];
                                    //for (int index = 0; index < requestDocumentModels.Count; index++)
                                    //{
                                    //    documents[index] = string.Concat("/BenefitRequestFiles/", requestDocumentModels[index].fileName);
                                    //    request.Documents = documents;
                                    //}
                                    request.Documents = requestDocumentModels.Select(d => d.fileName).ToArray();
                                }
                            }
                        }
                        requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                        requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels);
                    }
                }
                return View(manageRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowRequests(ManageRequest manageRequest)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                if (userRoles != null)
                {
                    if (userRoles.Contains("Admin"))
                    {
                        if (manageRequest.SelectedDepartmentId != -1)
                        {
                            requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.BenefitRequest.Employee.DepartmentId == manageRequest.SelectedDepartmentId).ToList();
                        }
                        else
                        {
                            requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows();
                        }
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                        //List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
                        //foreach (var dept in departmentModels)
                        //{
                        //    DepartmentAPI departmentAPI = new DepartmentAPI();
                        //    departmentAPI.Id = dept.Id;
                        //    departmentAPI.Name = dept.Name;
                        //    manageRequest.DepartmentModels.Add(departmentAPI);
                        //}
                    }
                    else if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR"))
                    {

                        requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                    }
                    if (requestWokflowModels.Count != 0)
                    {

                        if (manageRequest.employeeNumberSearch != 0)
                        {
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.EmployeeId == manageRequest.employeeNumberSearch).ToList();

                        }

                        if (manageRequest.SelectedRequestStatus != -1)
                        {
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId == manageRequest.SelectedRequestStatus).ToList();
                        }
                        if (manageRequest.SelectedTimingId != -1)
                        {
                            if (manageRequest.SelectedTimingId == 1)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate == DateTime.Today).ToList();
                            }
                            else if (manageRequest.SelectedTimingId == 2)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate == DateTime.Today.AddDays(-1)).ToList();
                            }
                            else if (manageRequest.SelectedTimingId == 3)
                            {
                                int days = DateTime.Now.DayOfWeek - DayOfWeek.Sunday;
                                DateTime pastDate = DateTime.Now.AddDays(-(days + 7));
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate >= pastDate && rw.BenefitRequest.RequestDate <= pastDate.AddDays(7)).ToList();
                            }
                            else if (manageRequest.SelectedTimingId == 4)
                            {

                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate.Month == DateTime.Today.Month - 1).ToList();
                            }
                        }
                        if (manageRequest.SelectedBenefitType != -1)
                        {
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.Benefit.BenefitTypeId == manageRequest.SelectedBenefitType).ToList();
                        }
                        requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                        requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                        manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;
                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels);
                    }

                }
                else
                {
                    ViewBag.Error = "You do not have any requests";
                }
                return View(manageRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public async Task<ActionResult> RequestResponse(long RequestId)
        {
            try
            {
                bool canResponse = false;
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();

                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(RequestId);
                //RequestWokflowModel requestWokflowModel =  _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(RequestId,employeeModel.EmployeeNumber);

                List<RequestWokflowModel> requestWokflowModels = _requestWorkflowService.GetRequestWorkflow(RequestId);
                RequestWokflowModel employeeRequestWokflowModel = requestWokflowModels.Where(rw => rw.EmployeeId == employeeModel.EmployeeNumber).First();
                if (employeeRequestWokflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                {
                    canResponse = true;
                }
                else if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Approved || employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Rejected)
                {
                    List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
                    List<int> workflowOrders = benefitWorkflowModels.Select(bw => bw.Order).ToList();
                    int workflowLevelsCount = benefitWorkflowModels.Count;
                    if (workflowLevelsCount > 1)
                    {
                        int order = benefitWorkflowModels.Where(bw => bw.RoleId == employeeRequestWokflowModel.RoleId).Select(bw => bw.Order).First();
                        int nextOrder = order + 1;
                        string nextRole = benefitWorkflowModels.Where(bw => bw.Order == nextOrder).Select(bw => bw.RoleId).First();
                        if (nextRole != null)
                        {
                            RequestWokflowModel nextEmployeeWorkflow = requestWokflowModels.Where(rw => rw.RoleId == nextRole).First();
                            if (nextEmployeeWorkflow.Status == (int)CommanData.BenefitStatus.Pending && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                            {
                                canResponse = true;
                            }
                            else
                            {
                                canResponse = false;
                            }
                        }
                        else
                        {
                            if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Rejected && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                            {
                                canResponse = true;
                            }
                            else if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Approved)
                            {

                                canResponse = false;
                            }
                        }
                    }
                    else
                    {
                        if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Rejected && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                        {
                            canResponse = true;
                        }
                        else if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Approved && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                        {
                            canResponse = true;

                        }
                        else
                        {
                            canResponse = false;
                        }
                    }
                }
                employeeRequestWokflowModel.ResonseStatuses = resonseStatuses;
                employeeRequestWokflowModel.canResponse = canResponse;
                employeeRequestWokflowModel.BenefitRequest.RequestDateString = employeeRequestWokflowModel.BenefitRequest.RequestDate.ToString("dd-MM-yyyy");
                employeeRequestWokflowModel.BenefitRequest.ExpectedDateFromString = employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom.ToString("dd-MM-yyyy");
                employeeRequestWokflowModel.BenefitRequest.ExpectedDateToString = employeeRequestWokflowModel.BenefitRequest.ExpectedDateTo.ToString("dd-MM-yyyy");
                return View(employeeRequestWokflowModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }
        public async Task<bool> AddResponse(long requestId, int status, string message)
        {
            try
            {
                bool result = false;
                string type = "Response";
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                RequestWokflowModel DBRequestWorkflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber, requestId);
                DBRequestWorkflowModel.ReplayDate = DateTime.Now;
                DBRequestWorkflowModel.IsVisible = true;
                DBRequestWorkflowModel.IsDelted = false;
                DBRequestWorkflowModel.UpdatedDate = DateTime.Now;
                DBRequestWorkflowModel.Notes = message;
                bool updateResult = false;
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(requestId);
                if (status == 2)
                {
                    DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    updateResult = _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel).Result;
                    if (updateResult == true)
                    {
                        updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                        if (updateResult == true)
                        {
                            ViewBag.Message = "Thank you for kind response";
                            result = true;
                            result = SendNotification(benefitRequestModel, DBRequestWorkflowModel, type).Result;
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Failed process";
                        result = false;

                    }
                }
                else if (status == 1)
                {
                    DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                    updateResult = _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel).Result;
                    if (updateResult == true)
                    {
                        List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
                        int benefitWorflowsCount = benefitWorkflowModels.Count;
                        int order = benefitWorkflowModels.Where(bw => bw.RoleId == DBRequestWorkflowModel.RoleId).Select(bw => bw.Order).First();
                        if (order == benefitWorflowsCount)
                        {
                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                            if (updateResult == true)
                            {
                                ViewBag.Message = "Thank you for kind response";
                                result = true;
                                result = SendNotification(benefitRequestModel, DBRequestWorkflowModel, type).Result;
                            }
                        }
                        else if (order <= benefitWorflowsCount)
                        {
                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.InProgress;
                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                            if (updateResult == true)
                            {
                                updateResult = SendNotification(benefitRequestModel, DBRequestWorkflowModel, type).Result;
                                int nextOrder = order + 1;
                                string messageResult = SendReuestToWhoIsConcern(benefitRequestModel.Id, nextOrder).Result;
                                ViewBag.Message = "Thank you for kind response";
                                result = true;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestResponse(RequestWokflowModel requestWokflowModel)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                RequestWokflowModel DBRequestWorkflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber, requestWokflowModel.BenefitRequestId);
                DBRequestWorkflowModel.ReplayDate = DateTime.Now;
                DBRequestWorkflowModel.IsVisible = true;
                DBRequestWorkflowModel.IsDelted = false;
                DBRequestWorkflowModel.UpdatedDate = DateTime.Now;
                DBRequestWorkflowModel.Notes = "Done";
                bool updateResult = false;
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(requestWokflowModel.BenefitRequestId);
                if (requestWokflowModel.RequestStatusSelectedId == 2)
                {
                    DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    updateResult = _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel).Result;
                    if (updateResult == true)
                    {
                        updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                        if (updateResult == true)
                        {
                            ViewBag.Message = "Thank you for kind response";
                            updateResult = SendNotification(benefitRequestModel, DBRequestWorkflowModel, "Response").Result;
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Failed process";
                    }
                }
                else if (requestWokflowModel.RequestStatusSelectedId == 1)
                {
                    DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                    updateResult = _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel).Result;
                    if (updateResult == true)
                    {
                        List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
                        int benefitWorflowsCount = benefitWorkflowModels.Count;
                        int order = benefitWorkflowModels.Where(bw => bw.RoleId == DBRequestWorkflowModel.RoleId).Select(bw => bw.Order).First();
                        if (order == benefitWorflowsCount)
                        {
                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                            if (updateResult == true)
                            {
                                ViewBag.Message = "Thank you for kind response";
                                updateResult = SendNotification(benefitRequestModel, DBRequestWorkflowModel, "Response").Result;

                            }

                        }
                        else if (order <= benefitWorflowsCount)
                        {
                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.InProgress;
                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                            if (updateResult == true)
                            {
                                int nextOrder = order + 1;
                                updateResult = SendNotification(benefitRequestModel, DBRequestWorkflowModel, "Response").Result;
                                string message = SendReuestToWhoIsConcern(benefitRequestModel.Id, nextOrder).Result;
                                ViewBag.Message = "Thank you for kind response";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return RedirectToAction("ShowRequests");


        }

        public async Task<bool> GroupRequestResponse(long groupId, int status)
        {
            try
            {
                bool updateResult = false;
                bool requestWorkflowResult = false;
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                GroupModel groupModel1 = _groupService.GetGroup(groupId);
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequestByGroupId((int)groupId).Result;
                RequestWokflowModel requestWokflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber, benefitRequestModel.Id);
                groupModel1.GroupStatus = "Closed";
                if (status == 2)
                {
                    groupModel1.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                }
                else if (status == 1)
                {
                    groupModel1.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                }
                updateResult = await _requestWorkflowService.UpdateRequestWorkflow(requestWokflowModel);

                if (updateResult == true)
                {
                    updateResult = await _benefitRequestService.UpdateBenefitRequest(benefitRequestModel);

                    if (updateResult == true)
                    {
                        updateResult = _groupService.UpdateGroup(groupModel1);

                        if (updateResult == true)
                        {
                            List<RequestWokflowModel> requestWorkflowModels = _requestWorkflowService.GetRequestWorkflow(benefitRequestModel.Id);
                            requestWorkflowModels = requestWorkflowModels.Where(rw => rw.EmployeeId != employeeModel.EmployeeNumber).ToList();
                            foreach (var requestWorkflow in requestWorkflowModels)
                            {
                                requestWorkflow.IsVisible = false;
                                requestWorkflow.IsDelted = true;
                                await _requestWorkflowService.UpdateRequestWorkflow(requestWorkflow);
                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return true;

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

        public JsonResult GetEmployeesCanRedeemThisGroupBenefit(string text, long benefitId)
        {
            //EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber);
            List<Participant> participants = new List<Participant>();

            //if (employeeModel != null)
            //{
            BenefitModel benefitModel = _BenefitService.GetBenefit(benefitId);
            int EmployeeGroupsCount = 0;
            List<EmployeeModel> employeeModels = _EmployeeService.GetAllEmployees().Result.ToList();
            List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetAllGroupEmployees().Result.ToList();
            groupEmployeeModels = _groupEmployeeService.GetAllGroupEmployees().Result.ToList().Where(ge => ge.Employee.FullName.Contains(text) == true).ToList();

            foreach (EmployeeModel employee in employeeModels)
            {
                EmployeeGroupsCount = groupEmployeeModels.Where(ge => ge.EmployeeId == employee.EmployeeNumber &&
                ge.Group.BenefitId == benefitId &&
               (ge.Group.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled ||
                ge.Group.RequestStatusId != (int)CommanData.BenefitStatus.Rejected)).ToList().Count;
                if (EmployeeGroupsCount < benefitModel.Times)
                {
                    Participant participant = new Participant();
                    participant.EmployeeNumber = employee.EmployeeNumber;
                    participant.FullName = employee.FullName;
                    participant.ProfilePicture = employee.ProfilePicture;
                    participants.Add(participant);
                }
            }
            //}
            return Json(new { items = participants });

            //return Json(new SelectList(participants, "EmployeeNumber", "FullName"));

        }
        public async Task<ActionResult> ShowAdminRequests()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                ManageRequest manageRequest = new ManageRequest();
                //if (userRoles != null)
                //{
                //    if (userRoles.Contains("Admin"))
                //    {
                //        requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                //    }
                //    else if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR"))
                //    {
                //        requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber).Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                //    }
                //if (requestWokflowModels.Count != 0)
                //{
                //    foreach (var request in requestWokflowModels)
                //    {
                //        if (request.BenefitRequest.Benefit.RequiredDocuments != null)
                //        {
                //            List<RequestDocumentModel> requestDocumentModels = _requestDocumentService.GetRequestDocuments(request.BenefitRequestId);
                //            if (requestDocumentModels != null)
                //            {
                //                //string[] documents = new string[requestDocumentModels.Count];
                //                //for (int index = 0; index < requestDocumentModels.Count; index++)
                //                //{
                //                //    documents[index] = string.Concat("/BenefitRequestFiles/", requestDocumentModels[index].fileName);
                //                //    request.Documents = documents;
                //                //}
                //                request.Documents = requestDocumentModels.Select(d => d.fileName).ToArray();
                //            }
                //        }
                //    }
                //requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                //requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                //manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels);
                //}
                //}
                manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;

                return View(manageRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowAdminRequests(ManageRequest manageRequest)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                if (userRoles != null)
                {
                    if (userRoles.Contains("Admin"))
                    {
                        if (manageRequest.SelectedDepartmentId != -1)
                        {
                            requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.BenefitRequest.Employee.DepartmentId == manageRequest.SelectedDepartmentId).ToList();
                        }
                        else
                        {
                            requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows();
                        }
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                        //List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
                        //foreach (var dept in departmentModels)
                        //{
                        //    DepartmentAPI departmentAPI = new DepartmentAPI();
                        //    departmentAPI.Id = dept.Id;
                        //    departmentAPI.Name = dept.Name;
                        //    manageRequest.DepartmentModels.Add(departmentAPI);
                        //}
                    }
                    else if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR"))
                    {

                        requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                    }
                    if (requestWokflowModels.Count != 0)
                    {
                        if (manageRequest.SelectedAll == false)
                        {


                            if (manageRequest.employeeNumberSearch != 0)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.EmployeeId == manageRequest.employeeNumberSearch).ToList();

                            }

                            if (manageRequest.SelectedRequestStatus != -1)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId == manageRequest.SelectedRequestStatus).ToList();
                            }
                            if (manageRequest.SelectedTimingId != -1)
                            {
                                if (manageRequest.SelectedTimingId == 1)
                                {
                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate == DateTime.Today).ToList();
                                }
                                else if (manageRequest.SelectedTimingId == 2)
                                {
                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate == DateTime.Today.AddDays(-1)).ToList();
                                }
                                else if (manageRequest.SelectedTimingId == 3)
                                {
                                    int days = DateTime.Now.DayOfWeek - DayOfWeek.Sunday;
                                    DateTime pastDate = DateTime.Now.AddDays(-(days + 7));
                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate >= pastDate && rw.BenefitRequest.RequestDate <= pastDate.AddDays(7)).ToList();
                                }
                                else if (manageRequest.SelectedTimingId == 4)
                                {

                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate.Month == DateTime.Today.Month - 1).ToList();
                                }
                            }
                            if (manageRequest.SelectedBenefitType != -1)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.Benefit.BenefitTypeId == manageRequest.SelectedBenefitType).ToList();
                            }
                        }
                        //requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                        //requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                        manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;
                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels);
                    }

                }
                else
                {
                    ViewBag.Error = "You do not have any requests";
                }
                return View(manageRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
    }

}
