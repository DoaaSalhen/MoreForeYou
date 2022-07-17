using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetAllEmployees();
        Task<bool> CreateEmployee(EmployeeModel model);
        Task<bool> UpdateEmployee(EmployeeModel model);
        bool DeleteEmployee(int id);
        EmployeeModel GetEmployee(long employeeNumber);
         EmployeeModel GetEmployeeByName(string name);

        Task<List<EmployeeModel>> GetEmployeesDataByDepartmentId(long DeptId);
        Task<EmployeeModel> GetEmployeeByUserId(string userId);

        EmployeeModel GetDepartmentManager(long departmentId);

        //Task<List<EmployeeModel>> GetEmployeeAuthority(long employeeNumber, String authorityName);
         EmployeeModel GetEmployeeBySapNumber(long sapNumber);

        EmployeeModel GetEmployeeById(string Id);

        Task<List<EmployeeModel>> GetAllEmployeeWhoCanIGive();

        LoginUser CreateLoginUser(EmployeeModel employeeModel);

        List<EmployeeModel> EmployeesSearch(FilterModel filterModel);

    }
}
