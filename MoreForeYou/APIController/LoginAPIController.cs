using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services;
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
    [Route("api/LoginAPI")]
    [ApiController]
    [Produces("application/json")]
    public class LoginAPIController : ControllerBase
    {
        private readonly IBenefitService _BenefitService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IEmployeeService _EmployeeService;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;
        private readonly IRoleService _roleService;
        private readonly IUserNotificationService _userNotificationService;
        public LoginAPIController(IBenefitService BenefitService,
            IBenefitWorkflowService BenefitWorkflowService,

            UserManager<AspNetUser> userManager,
             SignInManager<AspNetUser> signInManager,
            IEmployeeService EmployeeService,
            IBenefitRequestService benefitRequestService,
            IBenefitWorkflowService benefitWorkflowService,
            IRoleService roleService,
            IUserNotificationService userNotificationService
            )
        {
            _BenefitService = BenefitService;
            _userManager = userManager;
            _signInManager = signInManager;
            _EmployeeService = EmployeeService;
            _benefitRequestService = benefitRequestService;
            _benefitWorkflowService = benefitWorkflowService;
            _roleService = roleService;
            _userNotificationService = userNotificationService;
        }
        [HttpGet]
        [Route("All")]
        public EmployeeModel test()
        {
            EmployeeModel employee = _EmployeeService.GetEmployee(100);
            return employee;
        }

        [HttpPost("userLogin")]
        public async Task<ActionResult> UserLogin(LoginModel loginModel)
        {
            EmployeeModel employee = _EmployeeService.GetEmployee(loginModel.EmployeeNumber);
            if (employee != null)
            {
                AspNetUser aspNetUser = await _userManager.FindByIdAsync(employee.UserId);
                if (aspNetUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(aspNetUser.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(aspNetUser.Id);
                        HomeModel homeModel = _BenefitService.ShowAllBenefits(employeeModel);
                        homeModel.UserUnSeenNotificationCount = _userNotificationService.GetUserUnseenNotificationCount(employeeModel.EmployeeNumber);
                        homeModel.user.Email = aspNetUser.Email;
                        List<string> userRoles = _userManager.GetRolesAsync(aspNetUser).Result.ToList();
                        List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                        if (userRoles != null)
                        {
                            if (userRoles.Contains("Admin"))
                            {
                                homeModel.user.IsAdmin = true;
                            }
                            else
                            {
                                homeModel.user.IsAdmin = false;
                            }
                        }
                        
                        return Ok(new { Message = "Sucessful login", Data = homeModel });

                    }
                    else
                    {
                        return BadRequest(new { Message = "Invalid Email Or Password", Data = 0 });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Email not Exist", Data = 0 });
                }
            }
            else
            {
                return BadRequest(new { Message = "Employee Number not Exist", Data = 0 });
            }

        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult> UpdateToken(long employeeNumber, string newToken)
        {
            EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber);
            bool result = false;
            if (employeeModel != null)
            {
                //EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userId).Result;
                employeeModel.UserToken = newToken;
                result = _EmployeeService.UpdateEmployee(employeeModel).Result;
            }
            if (result == true)
            {
                return Ok(new { Message = "sucess process", Data = true });
            }
            else
            {
                return BadRequest(new { Message = "Failed Process, invalid Employee Data", Data = false });
            }
        }


        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(UserSetting userSetting)
        {
            EmployeeModel employee = _EmployeeService.GetEmployee(userSetting.employeeNumber);
            if (employee != null)
            {
                AspNetUser aspNetUser = await _userManager.FindByIdAsync(employee.UserId);
                if (aspNetUser != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(aspNetUser, userSetting.oldPassword, true);
                    if (result.Succeeded)
                    {
                        var result2 = await _userManager.ChangePasswordAsync(aspNetUser, userSetting.oldPassword, userSetting.newPassword);
                        if (result2.Succeeded)
                        {
                            return Ok(new { Message = "sucess process, your Password has been changed", Data = true });
                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed Process", Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Invaild data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Invaild data", Data = false });
                }
            }
            else
            {
                return BadRequest(new { Message = "Invaild data", Data = false });
            }

        }







    }
}
