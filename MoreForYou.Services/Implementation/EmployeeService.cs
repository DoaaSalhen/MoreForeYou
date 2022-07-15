using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models;

namespace MoreForYou.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee, long> _repository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepository<Employee, long> employeeRepository,
          ILogger<EmployeeService> logger, IMapper mapper)
        {
            _repository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public Task<bool> CreateEmployee(EmployeeModel model)
        {
            var employee = _mapper.Map<Employee>(model);
            try
            {
                _repository.Add(employee);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeModel>> GetAllEmployees()
        {
            try
            {
                var employees = _repository.Find(i => i.IsVisible == true,false,i=>i.Department,i=>i.Position,i=>i.Nationality, i=>i.Supervisor).ToList();
                var models = new List<EmployeeModel>();
                models = _mapper.Map<List<EmployeeModel>>(employees);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<List<EmployeeModel>> GetAllEmployeeWhoCanIGive()
        {
            try
            {
                var employees = _repository.Find(i => i.IsVisible == true).ToList();
                var models = new List<EmployeeModel>();
                models = _mapper.Map<List<EmployeeModel>>(employees);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public EmployeeModel GetDepartmentManager(long departmentId)
        {
            try
            {
               Employee employee = _repository.Find(e => e.IsVisible == true && e.isDeptManager == true && e.DepartmentId == departmentId, false, e => e.Department).First();
               EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                return employeeModel;

            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public EmployeeModel GetEmployee(long employeeNumber)
        {
            try
            {
                Employee employee = _repository.Find(e => e.EmployeeNumber == employeeNumber && e.IsVisible == true, false, e => e.Department, e => e.Position, e => e.Company, e => e.Nationality, e => e.Supervisor).First();
                EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                return employeeModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public EmployeeModel GetEmployeeByName(string name)
        {
            try
            {
               var employee = _repository.Find(e => e.FullName == name).First();
               EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                return employeeModel;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<EmployeeModel> GetEmployeeByUserId(string userId)
        {
            try
            {
                Employee employee = _repository.Find(i => i.IsVisible == true && i.UserId == userId, false, i=>i.Department, i=>i.Position, i=>i.Company, i=>i.Nationality, i => i.Supervisor).First();

                EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                return employeeModel;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<List<EmployeeModel>> GetEmployeesDataByDepartmentId(long DeptId)
        {
            try
            {
                var departments = _repository.Find(i => i.IsVisible == true && i.DepartmentId == DeptId).ToList();
                var models = new List<EmployeeModel>();
                models = _mapper.Map<List<EmployeeModel>>(departments);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public Task<bool> UpdateEmployee(EmployeeModel model)
        {
            var employee = _mapper.Map<Employee>(model);
            bool result = false;
            try
            {
                result = _repository.Update(employee);

                return Task<bool>.FromResult<bool>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public EmployeeModel GetEmployeeBySapNumber(long sapNumber)
        {
            try
            {
                Employee employee = _repository.Find(e => e.SapNumber == sapNumber).First();
                EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                return employeeModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public EmployeeModel GetEmployeeById(string Id)
        {
            try
            {
                Employee employee = _repository.Find(e => e.Id == Id).First();
                EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                return employeeModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public LoginUser CreateLoginUser(EmployeeModel employeeModel)
        {
            LoginUser user = new LoginUser();
            user.EmployeeName = employeeModel.FullName;
            user.PositionName = employeeModel.Position.Name;
            user.DepartmentName = employeeModel.Department.Name;
            user.EmployeeNumber = employeeModel.EmployeeNumber;
            user.BirthDate = employeeModel.BirthDate.ToString("yyyy-MM-dd");
            user.JoinDate = employeeModel.JoiningDate.ToString("yyyy-MM-dd");
            user.Address = employeeModel.Address;
            user.PhoneNumber = employeeModel.PhoneNumber;
            user.MaritalStatus = Enum.GetName(typeof(CommanData.MaritialStatus), employeeModel.MaritalStatus);
            user.Collar = Enum.GetName(typeof(CommanData.CollarTypes), employeeModel.Collar);
            user.Gender = Enum.GetName(typeof(CommanData.Gender), employeeModel.Gender);
            user.Company = employeeModel.Company.Code;
            user.Nationality = employeeModel.Nationality.Name;
            user.Id = employeeModel.Id;
            user.SupervisorName = employeeModel.Supervisor.FullName;
            user.SapNumber = employeeModel.SapNumber;
            user.ProfilePicture = employeeModel.ProfilePicture;
            user.PendingRequestsCount = employeeModel.PendingRequestsCount;
            user.hasRequests = employeeModel.hasRequests;
            return user;
        }
    }
}
