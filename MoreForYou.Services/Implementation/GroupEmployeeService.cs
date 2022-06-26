using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class GroupEmployeeService : IGroupEmployeeService
    {
            private readonly IRepository<GroupEmployee, long> _repository;
            private readonly ILogger<GroupEmployeeService> _logger;
            private readonly IMapper _mapper;

            public GroupEmployeeService(IRepository<GroupEmployee, long> Repository,
              ILogger<GroupEmployeeService> logger, IMapper mapper)
            {
                _repository = Repository;
                _logger = logger;
                _mapper = mapper;
            }
            public GroupEmployeeModel CreateGroupEmployee(GroupEmployeeModel model)
        {
            GroupEmployee groupEmployee = _mapper.Map<GroupEmployee>(model);

            try
            {
                var addedGroupEmployee = _repository.Add(groupEmployee);
                if (addedGroupEmployee != null)
                {
                    GroupEmployeeModel addedGroupEmployeeModel = new GroupEmployeeModel();
                    addedGroupEmployeeModel = _mapper.Map<GroupEmployeeModel>(addedGroupEmployee);
                    return addedGroupEmployeeModel;
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

        public async Task<List<GroupEmployeeModel>> GetAllGroupEmployees()
        {
            try
            {
                var employeeGroups = _repository.Find(ge=>ge.EmployeeId != 0, false, ge=>ge.Group, ge => ge.Employee);
                var models = new List<GroupEmployeeModel>();
                models = _mapper.Map<List<GroupEmployeeModel>>(employeeGroups);
                return models;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<List<GroupEmployeeModel>> GetGroupParticipants( long id)
        {
            try
            {
               var groupEmployees = _repository.Find(ge => ge.GroupId == id, false, ge => ge.Employee, ge => ge.Employee.Department, ge => ge.Employee.Position, ge => ge.Employee.Company);
                List<GroupEmployeeModel> groupEmployeeModels = _mapper.Map<List<GroupEmployeeModel>>(groupEmployees);

                return groupEmployeeModels;

            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<bool> UpdateGroupEmployee(GroupEmployeeModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GroupEmployeeModel>> GetGroupsByEmployeeId(long employeeId)
        {
            try
            {
                var groupEmployees = _repository.Find(ge => ge.EmployeeId == employeeId, false, ge => ge.Employee, ge => ge.Group, ge => ge.Group.RequestStatus);
                List<GroupEmployeeModel> groupEmployeeModels = _mapper.Map<List<GroupEmployeeModel>>(groupEmployees);

                return groupEmployeeModels;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public async Task<List<GroupEmployeeModel>> GetGroupsByBenefitId(long benefitId)
        {
            try
            {
                var groupEmployees = _repository.Find(ge => ge.EmployeeId != 0 && ge.Group.BenefitId == benefitId, false, ge => ge.Employee, ge => ge.Group, ge => ge.Group.RequestStatus);
                List<GroupEmployeeModel> groupEmployeeModels = _mapper.Map<List<GroupEmployeeModel>>(groupEmployees);

                return groupEmployeeModels;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

    }
}
