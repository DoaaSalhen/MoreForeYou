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
            filterModel.EmployeeModels = _EmployeeService.EmployeesSearch(filterModel);
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
        public async Task<ActionResult> Create(EmployeeModel Model)
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
                    Model.UserToken = "InitialToken";
                    var response = _EmployeeService.CreateEmployee(Model);
                    if(response.Result == true)
                    {
                        ViewBag.Message = "Employee Added Successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        AspNetUser aspNetUser = await _userManager.FindByIdAsync(addedUser.id);
                       var result = await _userManager.DeleteAsync(aspNetUser);
                        if(result.Succeeded)
                        {
                            ViewBag.Error = "Invalid data, Can not add your Employee";
                        }
                        else
                        {
                            ViewBag.Error = "Problem Occurred while add your employee Data, contact your support";
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid data, Can not add new User";
                }
                return View();
            }
            catch(Exception e)
            {
                ViewBag.Error = "Invalid data, Can not add your Employee";
                return View();
            }
        }

        public ActionResult Search(FilterModel filterModel)
        {
            filterModel.EmployeeModels = _EmployeeService.EmployeesSearch(filterModel);
            EmployeeModel employeeModel = new EmployeeModel();
            filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
            filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
            filterModel.PositionModels = _PostionService.GetAllPositions().Result;
            filterModel.CompanyModels = _companyService.GetAllCompanies();
            filterModel.collars = CommanData.Collars;
            filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Departmrnt" });
            filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });
            filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });
            filterModel.MartialStatusModels = CommanData.martialStatusModels;
            filterModel.genderModels = CommanData.genderModels;
            filterModel.BirthDate = DateTime.Today;
            //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
            //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));

            return View("Edit", filterModel);
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit()
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

        public ActionResult EditFromIndex(long id)
        {
            EmployeeModel employeeModel = new EmployeeModel();
            FilterModel filterModel = new FilterModel();
            filterModel.EmployeeNumber = id;
            filterModel.BirthDate = DateTime.Today;
            filterModel.SelectedDepartmentId = -1;
            filterModel.SelectedNationalityId = -1;
            filterModel.SelectedPositionId = -1;
            return RedirectToAction("Search",filterModel);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FilterModel filterModel)
        {
            try
            {
                //EmployeeModel DBemployeeModel = _EmployeeService.GetEmployee(employeeModel.EmployeeNumber);
                //DBemployeeModel.FullName = employeeModel.FullName;
                //DBemployeeModel.Id = employeeModel.Id;
                //DBemployeeModel.SapNumber = employeeModel.SapNumber;
                //DBemployeeModel.Address = employeeModel.Address;
                //DBemployeeModel.PhoneNumber = employeeModel.PhoneNumber;
                //DBemployeeModel.DepartmentId = employeeModel.DepartmentId;
                //DBemployeeModel.PositionId = employeeModel.PositionId;
                //DBemployeeModel.JoiningDate = employeeModel.JoiningDate;
                //DBemployeeModel.BirthDate = employeeModel.BirthDate;
                //DBemployeeModel.MaritalStatus = employeeModel.MaritalStatus;
                //DBemployeeModel.NationalityId = employeeModel.NationalityId;
                //DBemployeeModel.SupervisorId = employeeModel.SupervisorId;
                //DBemployeeModel.UpdatedDate = DateTime.Today;
                //DBemployeeModel.IsVisible = employeeModel.IsVisible;
                //_EmployeeService.UpdateEmployee(DBemployeeModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public Task<bool> EditEmployee(EditEmployee employeeModel)
        {
            try
            {
               EmployeeModel employee = _EmployeeService.GetEmployee(employeeModel.EmployeeNumber);
                employee.FullName = employeeModel.FullName;
                employee.Address = employeeModel.Address;
                employee.SapNumber = employeeModel.SapNumber;
                employee.Id = employeeModel.Id;
                employee.BirthDate = employeeModel.BirthDate;
                employee.Gender = employeeModel.Gender;
                employee.MaritalStatus = employeeModel.MaritalStatus;
                employee.DepartmentId = employeeModel.DepartmentId;
                employee.PositionId = employeeModel.PositionId;
                employee.CompanyId = employeeModel.CompanyId;
                employee.Collar = employeeModel.Collar;
                employee.isDeptManager = employeeModel.IsDeptManager;
                employee.PhoneNumber = employeeModel.PhoneNumber;
                employee.NationalityId = employeeModel.NationalityId;
                bool result = _EmployeeService.UpdateEmployee(employee).Result;
                return Task<bool>.FromResult(result);
            }
            catch(Exception e)
            {
                return Task<bool>.FromResult(false);
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

        public async Task<ActionResult> UserProfile(long EmployeeNumber)
        {
            EmployeeModel employeeModel =  _EmployeeService.GetEmployee(EmployeeNumber);
            employeeModel.GenderString = (CommanData.Gender)employeeModel.Gender;
            employeeModel.MaritalStatusString = (CommanData.MaritialStatus)employeeModel.MaritalStatus;
            employeeModel.CollarString = (CommanData.CollarTypes)employeeModel.Collar;
           var user = await _userService.GetUser(employeeModel.UserId);
            employeeModel.Email = user.Email;
            var supervisor = _EmployeeService.GetEmployee((long)employeeModel.SupervisorId);
            employeeModel.Supervisor = supervisor;
            return View(employeeModel);
        }

        //public async Task<ActionResult> MySetting()
        //{
        //    AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
        //    EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);

        //    UserSetting userSetting = new UserSetting();
        //    userSetting.imagePath = employeeModel.ProfilePicture;
        //    userSetting.userId = employeeModel.UserId;
        //    userSetting.employeeName = employeeModel.FullName;
        //    return View(userSetting);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePasswoard(UserSetting model)
        //{
        //    EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(model.userId);
        //    return View(employeeModel);
        //}
    }
}
