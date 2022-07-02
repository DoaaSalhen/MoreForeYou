using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoreForYou.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitAPIController : ControllerBase
    {
        private readonly IBenefitService _BenefitService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IEmployeeService _EmployeeService;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IRequestWorkflowService _requestWorkflowService;
        private readonly IRoleService _roleService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserNotificationService _userNotificationService;

        public BenefitAPIController(IBenefitService BenefitService,
            IBenefitWorkflowService BenefitWorkflowService,

            UserManager<AspNetUser> userManager,
             SignInManager<AspNetUser> signInManager,
            IEmployeeService EmployeeService,
            IBenefitRequestService benefitRequestService,
            IGroupEmployeeService groupEmployeeService,
            IRequestWorkflowService requestWorkflowService,
            IRoleService roleService,
            IDepartmentService departmentService,
            IUserNotificationService userNotificationService
            )
        {
            _BenefitService = BenefitService;
            _userManager = userManager;
            _signInManager = signInManager;
            _EmployeeService = EmployeeService;
            _benefitRequestService = benefitRequestService;
            _groupEmployeeService = groupEmployeeService;
            _requestWorkflowService = requestWorkflowService;
            _roleService = roleService;
            _departmentService = departmentService;
            _userNotificationService = userNotificationService;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost("GetBenefitDetails")]
        public async Task<ActionResult> GetBenefitDetails(long employeeNumber, long benefitId)
        {
            EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber);
            BenefitModel benefitModel = new BenefitModel();
            if (employeeModel != null)
            {
                BenefitAPIModel benefitAPIModel = _BenefitService.GetBenefitDetails(benefitId, employeeModel);
                if(benefitAPIModel != null)
                {
                    return Ok(new { Message = "Sucessful Process", Data = benefitModel });
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, benefit not found", Data = 0 });

                }

            }
            else
            {
                return BadRequest(new { Message = "Failed Process, wrong employee Data", Data = 0 });
            }

        }

        [HttpPost("WhoCanRedeemThisGroupBenefit")]
        public async Task<ActionResult> WhoCanRedeemThisGroupBenefit(long employeeNumber, long benefitId)
        {
            List<Participant> participants = new List<Participant>();
            participants = _BenefitService.GetEmployeesCanRedeemThisGroupBenefit(employeeNumber, benefitId);

            if (participants != null)
            {
                return Ok(new { Message = "Sucessful Process", Data = participants });

            }
            else
            {
                return BadRequest(new { Message = "Failed Process, employee not found", Data = 0 });
            }
        }


        [HttpPost("WhoCanIGiveThisBenefit")]
        public async Task<ActionResult> WhoCanIGiveThisBenefit(long employeeNumber, long benefitId)
        {
           List<Participant> participants = _BenefitService.GetEmployeesWhoCanIGiveThisBenefit(employeeNumber, benefitId);
           if (participants != null)
            {
                return Ok(new { Message = "Sucessful Process", Data = participants });
            }
            else
            {
                return BadRequest(new { Message = "Failed Process, employee not found", Data = 0 });
            }
        }
        //[HttpPost]
        //public async Task<ActionResult> AddNewRequest(Request  request)
        //{
        //    BenefitModel benefitModel = _BenefitService.GetBenefit(request.benefitId);
        //    if(benefitModel.BenefitTypeId == 2)
        //    {

        //    }
        //    else
        //    {

        //    }

        //}

        [HttpPost("ShowMyBenefits")]
        public async Task<ActionResult> ShowMyBenefits(int EmployeeNumber)
        {
            try
            {
                //AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployee(EmployeeNumber);
                List<BenefitAPIModel> benefitModels = _BenefitService.GetMyBenefits(CurrentEmployee.EmployeeNumber).ToList();
                if(benefitModels != null)
                {
                    return Ok(new { Message = "Sucessful Process", Data = benefitModels });

                }
                else
                {
                    return BadRequest(new { Message = "you do not have any benefits", Data = 0 });

                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = 0 });
            }
        }


        [HttpPost("ShowMyBenefitRequests")]
        public async Task<ActionResult> ShowMyBenefitRequests(long BenefitId, long EmployeeNumber)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployee(EmployeeNumber);
                long benefitTypeId = _BenefitService.GetBenefit(BenefitId).BenefitTypeId;
                List<Request> requests = _requestWorkflowService.GetMyBenefitRequests(CurrentEmployee.EmployeeNumber, BenefitId, benefitTypeId).ToList();
                MyRequests myRequests = new MyRequests();
                if(requests != null )
                {
                    requests =requests.OrderByDescending(r => r.Requestedat).ToList();
                    return Ok(new { Message = "Sucessful Process", Data = requests });

                }
                else
                {
                    return BadRequest(new { Message = "Failed Process", Data = 0 });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = 0 });
            }

        }

        [HttpPost ("ShowRequestsDefault")]
        public async Task<ActionResult> ShowRequests(long EmployeeNumber)
        {
            try
            {
                EmployeeModel employeeModel = _EmployeeService.GetEmployee(EmployeeNumber);
                AspNetUser CurrentUser = await _userManager.FindByIdAsync(employeeModel.UserId);
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
                        requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber).Where(rw=>rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();

                    }
                    //manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id).Result;
                    manageRequest.SelectedDepartmentId = -1;
                    manageRequest.SelectedRequestStatus = -1;
                    manageRequest.SelectedTimingId = -1;
                    manageRequest.SelectedBenefitType = -1;
                    if (requestWokflowModels.Count != 0)
                    {
                        requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                        requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels);
                    }
                }
                return Ok(new { Message = "Sucessful Process", Data = manageRequest });

            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = 0 });
            }

        }


        [HttpPost("ShowRequests")]
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
                        manageRequest.DepartmentModels = _departmentService.GetAllDepartments();
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
                        manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id).Result;
                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels);
                    }

                }
                else
                {
                    return Ok(new { Message = "user can not manage requests", Data = 0 });

                }
                return Ok(new { Message = "Sucessful Process", Data = manageRequest });

            }
            catch (Exception e)
            {
                return Ok(new { Message = "Failed Process", Data = 0 });

            }
        }

        [HttpPost("ShowNotifications")]
        public async Task<ActionResult> ShowNotifications(long employeeNumber)
        {
            try
            {
                List<NotificationAPIModel> notificationAPIModels = new List<NotificationAPIModel>();
                EmployeeModel employee = _EmployeeService.GetEmployee(employeeNumber);
               List<UserNotificationModel>  userNotificationModels = _userNotificationService.GetUserNotification(employee.UserId);
                if(userNotificationModels.Count > 0)
                {
                    List<NotificationAPIModel> NotificationAPIModels = new List<NotificationAPIModel>();
                    if (userNotificationModels.Count > 10)
                    {
                        userNotificationModels = userNotificationModels.OrderByDescending(un => un.CreatedDate).Take(10).ToList();
                    }
                    else
                    {
                        userNotificationModels = userNotificationModels.OrderByDescending(un => un.CreatedDate).Take(userNotificationModels.Count).ToList();

                    }

                     notificationAPIModels = _userNotificationService.CreateNotificationAPIModel(userNotificationModels);
                }
                else
                {
                     notificationAPIModels = new List<NotificationAPIModel>();

                }

                return Ok(new { Message = "Sucessful Process", Data =notificationAPIModels });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = 0 });

            }

        }


        [HttpPost("ConfirmRequest")]
        //[ValidateAntiForgeryToken]
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
                else if(benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
                {
                    result = _requestWorkflowService.ConfirmGroupRequest(request, CurrentUser.Id, benefitModel).Result;

                }
                if (result.Contains("Success Process"))
                {
                    return Ok(new { Message = "Sucessful Process", Data = request });
                }
                else
                {
                    return BadRequest(new { Message = result, Data = 0 });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = 0 });

            }
        }

        [HttpPost("UploadRequestDocuments")]
        public async Task<IActionResult> UploadRequestDocuments( long requestNumber, List<IFormFile> files)
        {
            string message = await _requestWorkflowService.AddDocumentsToRequest(requestNumber, files);
            if(message.Contains("Success Process"))
            {
                return Ok ( new {Message = "Sucessful Process", Data = true});
            }
            else
            {
                return BadRequest(new { Message = "Failed Process", Data = false });

            }
        }


        [HttpPost("updatePrfilePicture")]
        public async Task<IActionResult> updatePrfilePicture(string userId, IFormFile file)
        {
            string fileName = "";
            bool result = false;
            if(file.Length > 0)
            {
                fileName = await _requestWorkflowService.UploadedImageAsync(file, "images/userProfile");
                if(fileName != "")
                {
                    EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(userId);
                    employeeModel.ProfilePicture = fileName;
                    result = _EmployeeService.UpdateEmployee(employeeModel).Result;
                }
            }
            if (result == true)
            {
                return Ok(new { Message = "profile picture updated Successfully", Data = true });
            }
            else
            {
                return BadRequest(new { Message = "Failed Process", Data = false });
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
