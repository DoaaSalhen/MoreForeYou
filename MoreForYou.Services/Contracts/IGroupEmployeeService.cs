using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
   public interface IGroupEmployeeService
    {
        Task<List<GroupEmployeeModel>> GetAllGroupEmployees();
        GroupEmployeeModel CreateGroupEmployee(GroupEmployeeModel model);
        Task<bool> UpdateGroupEmployee (GroupEmployeeModel model);
        Task<List<GroupEmployeeModel>> GetGroupParticipants(long id);
        Task<List<GroupEmployeeModel>> GetGroupsByEmployeeId(long employeeId);

        Task<List<GroupEmployeeModel>> GetGroupsByBenefitId(long benefitId);


    }
}
