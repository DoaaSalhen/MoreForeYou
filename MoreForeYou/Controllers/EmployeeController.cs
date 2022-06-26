using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Implementation;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _EmployeeService;
        private readonly IPositionService _PostionService;
        private readonly IDepartmentService _DepartmentService;
        private readonly INationalityService _NationalityService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<AspNetUser> _userManager;


        List<GenderModel> genderList = new List<GenderModel>()
            {
                new GenderModel { Id=1, Name='M'},
                new GenderModel {Id= 2, Name='F'}
            };

        public List<Collar> Collars = new List<Collar>()
        {
            new Collar { Id = 1, Name = "White Collar" },
            new Collar { Id = 1, Name = "Blue Collar" }

        };
        public EmployeeController(IEmployeeService EmployeeService,
            IDepartmentService DepartmentService,
            IPositionService PositionService,
            INationalityService NationalityService,
            IUserService userService,
            IRoleService roleService,
            ICompanyService companyService,
            UserManager<AspNetUser> userManager)
        {
            _EmployeeService = EmployeeService;
            _DepartmentService = DepartmentService;
            _PostionService = PositionService;
            _NationalityService = NationalityService;
            _userService = userService;
            _roleService = roleService;
            _companyService = companyService;
            _userManager = userManager;
        }

        public JsonResult DepartmentFilter(long id)
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
                if(Supervisor != null)
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

        public bool CheckUniqueEmail(string email)
        {
            bool result = false;
            try
            {
                if (email != "")
                {

                   UserModel userModel = _userService.SearchEmail(email).Result;
                    if(userModel == null)
                    { 
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public bool CheckUniqueSapNumber(long SapNumber)
        {
            bool result = false;
            try
            {
                if (SapNumber != 0)
                {
                   EmployeeModel employeeModel = _EmployeeService.GetEmployeeBySapNumber(SapNumber);


                    if (employeeModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public bool CheckUniqueEmployeeNumber(long EmployeeNumber)
        {
            bool result = false;
            try
            {
                if (EmployeeNumber != 0)
                {
                    EmployeeModel employeeModel = _EmployeeService.GetEmployee(EmployeeNumber);


                    if (employeeModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public bool CheckUniqueId(string Id)
        {
            bool result = false;
            try
            {
                if (Id != "")
                {
                    EmployeeModel employeeModel = _EmployeeService.GetEmployeeById(Id);
                    if (employeeModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            EmployeeModel employeeModel = new EmployeeModel();
            FilterModel filterModel = new FilterModel();
            filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
            filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
            filterModel.PositionModels = _PostionService.GetAllPositions().Result;
            filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Departmrnt" });
            filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });

            filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });

            filterModel.BirthDate = DateTime.Today;
            //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
            //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));

            return View(filterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(FilterModel filterModel)
        {
            List<EmployeeModel> employeeModels = _EmployeeService.GetAllEmployees().Result;
            if(filterModel.EmployeeName != null)
            {
                employeeModels = employeeModels.Where(e => e.FullName.Contains(filterModel.EmployeeName)).ToList();
            }
            if(filterModel.EmployeeNumber != 0)
            {
                employeeModels = employeeModels.Where(e => e.EmployeeNumber == filterModel.EmployeeNumber).ToList();
            }
            if (filterModel.SapNumber != 0)
            {
                employeeModels = employeeModels.Where(e => e.SapNumber == filterModel.SapNumber).ToList();
            }
            if (filterModel.Id != null)
            {
                employeeModels = employeeModels.Where(e => e.Id == filterModel.Id).ToList();
            }
            if (filterModel.Email != null)
            {
                employeeModels = employeeModels.Where(e => e.Email.Contains(filterModel.Email)).ToList();
            }
            if (filterModel.BirthDate != DateTime.Today)
            {
                employeeModels = employeeModels.Where(e => e.BirthDate== filterModel.BirthDate).ToList();
            }
            if (filterModel.SelectedDepartmentId != -1)
            {
                employeeModels = employeeModels.Where(e => e.DepartmentId == filterModel.SelectedDepartmentId).ToList();
            }
            if (filterModel.SelectedPositionId != -1)
            {
                employeeModels = employeeModels.Where(e => e.PositionId == filterModel.SelectedPositionId).ToList();
            }
            if (filterModel.SelectedNationalityId != -1)
            {
                employeeModels = employeeModels.Where(e => e.NationalityId == filterModel.SelectedNationalityId).ToList();
            }
            filterModel.EmployeeModels = employeeModels;
            EmployeeModel employeeModel = new EmployeeModel();
            filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
            filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
            filterModel.PositionModels = _PostionService.GetAllPositions().Result;
            filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Departmrnt" });
            filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });

            filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });

            filterModel.BirthDate = DateTime.Today;
            //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
            //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));

            return View(filterModel);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            EmployeeModel employeeModel = new EmployeeModel();
            employeeModel.DepartmentModels = _DepartmentService.GetAllDepartments();
            employeeModel.PositionModels = _PostionService.GetAllPositions().Result;
            employeeModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
            employeeModel.genderModels = genderList;
            employeeModel.RoleModels = _roleService.GetAllRoles().Result;
            employeeModel.BirthDate = DateTime.Today;
            employeeModel.JoiningDate = DateTime.Today;
            employeeModel.Collars = Collars;
            employeeModel.Companies = _companyService.GetAllCompanies();
           // employeeModel.Companies = 
            return View(employeeModel);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeModel Model)
        {
            try
            {
                bool hasRole = false;
                UserModel user = new UserModel();
                user.Email = Model.Email;
                user.Password = Model.Password;
                if (Model.AsignedRolesNames != null)
                {
                    user.AsignedRolesNames = Model.AsignedRolesNames;
                    hasRole = true;
                }
                UserModel addedUser = _userService.CreateUser(user, hasRole).Result;
                if(addedUser != null)
                {
                    Model.CreatedDate = DateTime.Now;
                    Model.UpdatedDate = DateTime.Now;
                    Model.IsVisible = true;
                    Model.IsDelted = false;
                    Model.UserId = addedUser.id;
                    var response = _EmployeeService.CreateEmployee(Model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
           EmployeeModel employeeModel = _EmployeeService.GetEmployee(id);
            employeeModel.DepartmentModels = _DepartmentService.GetAllDepartments();
            employeeModel.PositionModels = _PostionService.GetAllPositions().Result;
            employeeModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
            employeeModel.genderModels = genderList;
            employeeModel.EmployeeModels = _EmployeeService.GetEmployeesDataByDepartmentId(employeeModel.DepartmentId).Result;
            employeeModel.RoleModels = _roleService.GetAllRoles().Result;
            return View(employeeModel);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeModel employeeModel)
        {
            try
            {
                EmployeeModel DBemployeeModel = _EmployeeService.GetEmployee(employeeModel.EmployeeNumber);
                DBemployeeModel.FullName = employeeModel.FullName;
                DBemployeeModel.Id = employeeModel.Id;
                DBemployeeModel.SapNumber = employeeModel.SapNumber;
                DBemployeeModel.Address = employeeModel.Address;
                DBemployeeModel.PhoneNumber = employeeModel.PhoneNumber;
                DBemployeeModel.DepartmentId = employeeModel.DepartmentId;
                DBemployeeModel.PositionId = employeeModel.PositionId;
                DBemployeeModel.JoiningDate = employeeModel.JoiningDate;
                DBemployeeModel.BirthDate = employeeModel.BirthDate;
                DBemployeeModel.MaritalStatus = employeeModel.MaritalStatus;
                DBemployeeModel.NationalityId = employeeModel.NationalityId;
                DBemployeeModel.SupervisorId = employeeModel.SupervisorId;
                DBemployeeModel.UpdatedDate = DateTime.Today;
                DBemployeeModel.IsVisible = employeeModel.IsVisible;
                _EmployeeService.UpdateEmployee(DBemployeeModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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

        
        //public async Task<ActionResult> ChangeSetting()
        //{

        //}

        public async Task<ActionResult> UserProfile(string userid)
        {
            EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(userid);
            employeeModel.GenderString = (CommanData.Gender)employeeModel.Gender;
            employeeModel.MaritalStatusString = (CommanData.MaritialStatus)employeeModel.MaritalStatus;
            employeeModel.CollarString = (CommanData.CollarTypes)employeeModel.Collar;
           var user = _userService.GetUser(employeeModel.UserId);
            employeeModel.Email = user.Result.Email;
            var supervisor = _EmployeeService.GetEmployee((long)employeeModel.SupervisorId);
            employeeModel.Supervisor = supervisor;
            return View(employeeModel);
        }

        public async Task<ActionResult> MySetting()
        {
            AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
            EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);

            UserSetting userSetting = new UserSetting();
            userSetting.imagePath = employeeModel.ProfilePicture;
            userSetting.userId = employeeModel.UserId;
            userSetting.employeeName = employeeModel.FullName;
            return View(userSetting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswoard(UserSetting model)
        {
            EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(model.userId);
            return View(employeeModel);
        }
    }
}
