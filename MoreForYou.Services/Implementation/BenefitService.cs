using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MoreForYou.Services.Implementation
{
    public class BenefitService : IBenefitService
    {
        private readonly IRepository<Benefit, long> _repository;
        private readonly ILogger<BenefitService> _logger;
        private readonly IMapper _mapper;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IEmployeeService _employeeService;
        private readonly IRequestWorkflowService _requestWorkflowService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;

        public BenefitService(IRepository<Benefit, long> benefitRepository,
          ILogger<BenefitService> logger,
          IMapper mapper,
          IBenefitRequestService benefitRequestService,
          IGroupEmployeeService groupEmployeeService,
          IEmployeeService employeeService,
          IRequestWorkflowService requestWorkflowService,
          IBenefitWorkflowService benefitWorkflowService
          )
        {
            _repository = benefitRepository;
            _logger = logger;
            _mapper = mapper;
            _benefitRequestService = benefitRequestService;
            _groupEmployeeService = groupEmployeeService;
            _employeeService = employeeService;
            _requestWorkflowService = requestWorkflowService;
            _benefitWorkflowService = benefitWorkflowService;
        }

        public BenefitModel CreateBenefit(BenefitModel model)
        {
            var benefit = _mapper.Map<Benefit>(model);

            try
            {
                var addedBenefit = _repository.Add(benefit);
                if (addedBenefit != null)
                {
                    BenefitModel addedBenefitModel = new BenefitModel();
                    addedBenefitModel = _mapper.Map<BenefitModel>(addedBenefit);
                    return addedBenefitModel;
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

        public bool DeleteBenefit(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BenefitModel>> GetAllBenefits()
        {
            try
            {
                var benefits = _repository.Find(i => i.IsVisible == true, false, b => b.BenefitType).ToList();
                var models = new List<BenefitModel>();
                models = _mapper.Map<List<BenefitModel>>(benefits);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public BenefitModel GetBenefit(long id)
        {
            try
            {
                var benefit = _repository.Find(b => b.Id == id && b.IsVisible == true, false, b => b.BenefitType).First();
                BenefitModel benefitModel = _mapper.Map<BenefitModel>(benefit);
                return benefitModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<List<BenefitModel>> GetBenefitByName(BenefitModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBenefit(BenefitModel model)
        {
            var benefit = _mapper.Map<Benefit>(model);

            try
            {
                _repository.Update(benefit);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }


        public List<BenefitModel> BenefitsUserCanRedeem(List<BenefitModel> benefitModels, EmployeeModel employeeModel)
        {
            int times = 0;
            int HoldedRequests = 0;
            int employeeWorkDuration = 0;
            int employeeAge = 0;
            int groupBenefitTimes = 0;
            int groupHoldRequest = 0;
            List<GroupEmployeeModel> employeeBenefitGroup = new List<GroupEmployeeModel>();
            foreach (BenefitModel benefitModel in benefitModels)
            {
                employeeWorkDuration = DateTime.Now.Year - employeeModel.JoiningDate.Year;
                employeeAge = DateTime.Now.Year - employeeModel.BirthDate.Year;
                List<BenefitRequestModel> employeeBenefitRequestModels = _benefitRequestService.GetBenefitRequestByEmployeeId(employeeModel.EmployeeNumber, benefitModel.Id).Result.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Approved || r.RequestStatusId == (int)CommanData.BenefitStatus.Pending || r.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).ToList();

                var employeeGroups = _groupEmployeeService.GetGroupsByEmployeeId(employeeModel.EmployeeNumber).Result;
                if (employeeGroups != null)
                {
                    employeeBenefitGroup = employeeGroups.Where(eg => eg.Group.BenefitId == benefitModel.Id) != null ? employeeGroups.Where(eg => eg.Group.BenefitId == benefitModel.Id).ToList() : null;
                    if (employeeBenefitGroup != null)
                    {
                        groupBenefitTimes = employeeBenefitGroup.Where(eg => eg.Group.RequestStatusId == (int)CommanData.BenefitStatus.Approved).ToList().Count;
                        groupHoldRequest = employeeBenefitGroup.Where(r => r.Group.RequestStatusId == (int)CommanData.BenefitStatus.Pending || r.Group.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).Count();
                    }

                }
                if (employeeBenefitRequestModels.Count > 0 || employeeBenefitGroup != null)
                {
                    times = employeeBenefitRequestModels.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Approved).Count() + groupBenefitTimes;
                    HoldedRequests = employeeBenefitRequestModels.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Pending || r.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).Count() + groupHoldRequest;
                }

                benefitModel.EmployeeCanRedeem = false;
                if (benefitModel.Year == DateTime.Now.Year)
                {
                    if (benefitModel.Times > times && HoldedRequests == 0)
                    {
                        if (benefitModel.Collar == -1 || (benefitModel.Collar != -1 && employeeModel.Collar == benefitModel.Collar))
                        {
                            if (benefitModel.gender == -1 || (benefitModel.gender != -1 && employeeModel.Gender == benefitModel.gender))
                            {
                                if (benefitModel.MaritalStatus == -1 || (benefitModel.MaritalStatus != -1 && employeeModel.MaritalStatus == benefitModel.MaritalStatus))
                                {
                                    if (benefitModel.WorkDuration == 0 || (benefitModel.WorkDuration != 0 && employeeWorkDuration >= benefitModel.WorkDuration))
                                    {
                                        if (benefitModel.Age != 0)
                                        {
                                            if ((benefitModel.AgeSign == ">" && employeeAge > benefitModel.Age) ||
                                                (benefitModel.AgeSign == "<" && employeeAge < benefitModel.Age) ||
                                                (benefitModel.AgeSign == "=" && employeeAge == benefitModel.Age))
                                            {
                                                benefitModel.EmployeeCanRedeem = true;
                                            }
                                        }
                                        else
                                        {
                                            benefitModel.EmployeeCanRedeem = true;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return benefitModels;
        }

        public BenefitConditionsAndAvailable CreateBenefitConditions(BenefitModel benefitModel, EmployeeModel employeeModel)
        {
            Dictionary<string, string> BenefitConditions = new Dictionary<string, string>();
            Dictionary<string, bool> ConditionsApplicable = new Dictionary<string, bool>();

            if (benefitModel.Age != 0)
            {
                //BenefitConditions.Add("Age " + benefitModel.AgeSign + benefitModel.Age);
                BenefitConditions.Add("Age", "Age " + benefitModel.AgeSign + benefitModel.Age);
                int employeeAge = DateTime.Now.Year - employeeModel.BirthDate.Year;
                if ((benefitModel.AgeSign == ">" && employeeAge > benefitModel.Age) ||
                                                (benefitModel.AgeSign == "<" && employeeAge < benefitModel.Age) ||
                                                (benefitModel.AgeSign == "=" && employeeAge == benefitModel.Age))
                {
                    ConditionsApplicable.Add("Age", true);

                }
                else
                {
                    ConditionsApplicable.Add("Age", false);

                }
            }
            //else
            //{
            //    BenefitConditions.Add("Age : Any");
            //}
            BenefitConditions.Add("Type", benefitModel.BenefitType.Name);
            ConditionsApplicable.Add("Type", true);

            if (benefitModel.WorkDuration != 0)
            {
                BenefitConditions.Add("WorkDuration", "Work Duration >= " + benefitModel.WorkDuration);
                int employeeWorkDuartion = DateTime.Now.Year - employeeModel.JoiningDate.Year;
                if (employeeWorkDuartion >= benefitModel.WorkDuration)
                {
                    ConditionsApplicable.Add("WorkDuration", true);

                }
                else
                {
                    ConditionsApplicable.Add("WorkDuration", false);
                }

            }
            if (benefitModel.gender != (int)CommanData.Gender.Any)
            {
                BenefitConditions.Add("Gender", "" + (CommanData.Gender)benefitModel.gender);
                if (employeeModel.Gender == benefitModel.gender)
                {
                    ConditionsApplicable.Add("Gender", true);

                }
                else
                {
                    ConditionsApplicable.Add("Gender", false);
                }

            }
            if (benefitModel.MaritalStatus != (int)CommanData.MaritialStatus.Any)
            {
                BenefitConditions.Add("MaritalStatus", "" + (CommanData.MaritialStatus)benefitModel.MaritalStatus);
                if (employeeModel.MaritalStatus == benefitModel.MaritalStatus)
                {
                    ConditionsApplicable.Add("MaritalStatus", true);

                }
                else
                {
                    ConditionsApplicable.Add("MaritalStatus", false);
                }
            }
            if (benefitModel.Collar != (int)CommanData.CollarTypes.Any)
            {
                BenefitConditions.Add("PayrollArea", "" + (CommanData.CollarTypes)benefitModel.Collar);
                if (employeeModel.Collar == benefitModel.Collar)
                {
                    ConditionsApplicable.Add("PayrollArea", true);

                }
                else
                {
                    ConditionsApplicable.Add("PayrollArea", false);
                }
            }
            if (benefitModel.DateToMatch != "Any" && (benefitModel.DateToMatch == "Birth Date" || benefitModel.DateToMatch == "Join Date"))
            {
                BenefitConditions.Add("DateToMatch", "Benefit Redeemation date must match with your :" + benefitModel.DateToMatch);
                ConditionsApplicable.Add("DateToMatch", true);

            }
            else if (benefitModel.DateToMatch != "Any" && (benefitModel.DateToMatch == "Certain Date"))
            {
                BenefitConditions.Add("DateToMatch", "Benefit Redeemation date must be at :" + benefitModel.CertainDate);
                ConditionsApplicable.Add("DateToMatch", true);
            }

            //else
            //{
            //    BenefitConditions.Add("Benefit Redeemation can be at any date you desired");
            //}
            if (benefitModel.RequiredDocuments != null)
            {
                BenefitConditions.Add("RequiredDocuments", "Required Documents are " + benefitModel.RequiredDocuments);
                ConditionsApplicable.Add("RequiredDocuments", true);

            }

            if (benefitModel.BenefitTypeId == 3)
            {
                BenefitConditions.Add("MinParticipant", "" + benefitModel.MinParticipant);
                BenefitConditions.Add("MaxParticipant", "" + benefitModel.MaxParticipant);
                ConditionsApplicable.Add("MinParticipant", true);
                ConditionsApplicable.Add("MaxParticipant", true);

            }
            BenefitConditionsAndAvailable benefitConditionsAndAvailable = new BenefitConditionsAndAvailable();
            benefitConditionsAndAvailable.BenefitApplicable = ConditionsApplicable;
            benefitConditionsAndAvailable.BenefitConditions = BenefitConditions;
            return benefitConditionsAndAvailable;

        }

        public BenefitAPIModel CreateBenefitAPIModel(BenefitModel model)
        {
            try
            {
                BenefitAPIModel benefitAPIModel = new BenefitAPIModel();
                benefitAPIModel.Id = model.Id;
                benefitAPIModel.Name = model.Name;
                benefitAPIModel.Description = model.Description;
                benefitAPIModel.BenefitCard = model.BenefitCard;
                benefitAPIModel.Times = model.Times;
                benefitAPIModel.numberOfDays = model.numberOfDays;
                benefitAPIModel.DateToMatch = model.DateToMatch;
                benefitAPIModel.CertainDate = model.CertainDate;
                benefitAPIModel.BenefitType = model.BenefitType.Name;
                benefitAPIModel.BenefitConditions = model.BenefitConditions;
                benefitAPIModel.BenefitApplicable = model.BenefitApplicable;
                benefitAPIModel.BenefitWorkflows = model.BenefitWorkflows;
                benefitAPIModel.EmployeeCanRedeem = model.EmployeeCanRedeem;
                benefitAPIModel.MaxParticipant = model.MaxParticipant;
                benefitAPIModel.MinParticipant = model.MinParticipant;
                benefitAPIModel.IsAgift = model.IsAgift;
                benefitAPIModel.LastStatus = model.LastStatus;
                //benefitAPIModel.benefitStatses = model.benefitStatses;
                benefitAPIModel.totalRequestsCount = model.totalRequestsCount;
                if (model.RequiredDocumentsArray != null)
                {
                    benefitAPIModel.RequiredDocumentsArray = model.RequiredDocumentsArray;
                }
                return benefitAPIModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public List<BenefitAPIModel> GetMyBenefits(long employeeNumber)
        {
            try
            {
                List<BenefitModel> benefitModels = new List<BenefitModel>();
                var benefitRequestModels1 = _benefitRequestService.GetBenefitRequestByEmployeeId(employeeNumber).Result;
                List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupsByEmployeeId(employeeNumber).Result;
                if (groupEmployeeModels != null || benefitRequestModels1.Count != 0)
                {

                    if (benefitRequestModels1.Count != 0)
                    {
                        benefitRequestModels1 = benefitRequestModels1.OrderByDescending(r => r.CreatedDate).ToList();
                        List<BenefitRequestModel> benefitRequestModels = benefitRequestModels1.Where(r => r.GroupId == null).ToList();

                        var benefitRequestModelsGroup = benefitRequestModels.GroupBy(BR => BR.Benefit.Id).ToList();
                        foreach (var group in benefitRequestModelsGroup)
                        {
                            BenefitModel benefitModel = GetBenefit(group.Key);

                            List<BenefitRequestModel> benefitRequestModels2 = group.AsEnumerable().ToList();
                            //List<BenefitStats> benefitStats = GetIndividualBenefitStats(benefitRequestModels2).ToList();
                            //benefitModel.benefitStatses = benefitStats;
                            benefitModel.totalRequestsCount = benefitRequestModels2.Count;
                            benefitModel.LastStatus = group.First().RequestStatus.Name;
                            benefitModel.LastRequetedDate = group.First().CreatedDate;
                            benefitModels.Add(benefitModel);
                        }
                    }
                    if (groupEmployeeModels != null)
                    {
                        var groupEmployeeModelGroups = groupEmployeeModels.OrderByDescending(g => g.Group.CreatedDate).GroupBy(g => g.Group.BenefitId).ToList();//.Select(g => g.Select(g => g.Group.Benefit));
                        foreach (var groups in groupEmployeeModelGroups)
                        {
                            List<BenefitRequestModel> GroupbenefitRequestes = new List<BenefitRequestModel>();
                            BenefitModel benefitModel = GetBenefit(groups.Key);

                            foreach (var group in groups)
                            {
                                BenefitRequestModel GroupbenefitRequest = _benefitRequestService.GetBenefitRequestByGroupId(group.Group.Id).Result;
                                GroupbenefitRequestes.Add(GroupbenefitRequest);

                            }
                            //List<BenefitStats> benefitStats = GetIndividualBenefitStats(GroupbenefitRequestes).ToList();
                            benefitModel.totalRequestsCount = GroupbenefitRequestes.Count;
                            benefitModel.LastStatus = GroupbenefitRequestes.First().RequestStatus.Name;
                            benefitModel.LastRequetedDate = GroupbenefitRequestes.First().CreatedDate;

                            benefitModels.Add(benefitModel);
                        }
                    }
                    List<BenefitAPIModel> benefitAPIModels = new List<BenefitAPIModel>();
                    if (benefitModels.Count != 0)
                    {
                        benefitModels = benefitModels.OrderByDescending(b => b.LastRequetedDate).ToList();

                        foreach (BenefitModel model in benefitModels)
                        {
                            BenefitAPIModel benefitAPIModel = CreateBenefitAPIModel(model);
                            benefitAPIModels.Add(benefitAPIModel);
                        }
                        return benefitAPIModels;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public List<Participant> GetEmployeesCanRedeemThisGroupBenefit(long employeeNumber, long benefitId)
        {
            EmployeeModel employeeModel = _employeeService.GetEmployee(employeeNumber);
            List<Participant> participants = new List<Participant>();

            if (employeeModel != null)
            {
                BenefitModel benefitModel = GetBenefit(benefitId);
                int EmployeeGroupsCount = 0;
                int EmployeePendingGroupsCount = 0;
                int employeeWorkDuration = 0;
                int employeeAge = 0;
                bool flag = false;
                List<EmployeeModel> employeeModels = _employeeService.GetAllEmployees().Result.ToList();
                foreach (EmployeeModel employee in employeeModels)
                {
                    employeeWorkDuration = DateTime.Now.Year - employeeModel.JoiningDate.Year;
                    employeeAge = DateTime.Now.Year - employeeModel.BirthDate.Year;

                    if (benefitModel.Year == DateTime.Now.Year)
                    {
                        if (benefitModel.Collar == -1 || (benefitModel.Collar != -1 && employeeModel.Collar == benefitModel.Collar))
                        {
                            if (benefitModel.gender == -1 || (benefitModel.gender != -1 && employeeModel.Gender == benefitModel.gender))
                            {
                                if (benefitModel.MaritalStatus == -1 || (benefitModel.MaritalStatus != -1 && employeeModel.MaritalStatus == benefitModel.MaritalStatus))
                                {
                                    if (benefitModel.WorkDuration == 0 || (benefitModel.WorkDuration != 0 && employeeWorkDuration >= benefitModel.WorkDuration))
                                    {
                                        if (benefitModel.Age != 0)
                                        {
                                            if ((benefitModel.AgeSign == ">" && employeeAge > benefitModel.Age) ||
                                                (benefitModel.AgeSign == "<" && employeeAge < benefitModel.Age) ||
                                                (benefitModel.AgeSign == "=" && employeeAge == benefitModel.Age))
                                            {
                                                flag = true;
                                            }
                                            else
                                            {
                                                flag = false;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                    if (flag == true)
                    {
                        List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupsByEmployeeId(employee.EmployeeNumber).Result;

                        var EmployeesGroup = groupEmployeeModels.Where(
                                                     ge => ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.Approved ||
                                                      ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.InProgress ||
                                                      ge.Group.RequestStatusId == (int)CommanData.BenefitStatus.Pending);
                        List<GroupEmployeeModel> EmployeeWithPendingGroup = groupEmployeeModels.Where(g => g.EmployeeId == employee.EmployeeNumber && g.Group.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                        if (EmployeesGroup != null)
                        {
                            EmployeeGroupsCount = EmployeesGroup.ToList().Count;
                            EmployeePendingGroupsCount = EmployeeWithPendingGroup.Count();
                            if (EmployeeGroupsCount < benefitModel.Times && EmployeePendingGroupsCount == 0)
                            {
                                Participant participant = new Participant();
                                participant.EmployeeNumber = employee.EmployeeNumber;
                                participant.FullName = employee.FullName;
                                //participant.ProfilePicture = employee.ProfilePicture;
                                participants.Add(participant);
                            }
                        }
                        else
                        {
                            Participant participant = new Participant();
                            participant.EmployeeNumber = employee.EmployeeNumber;
                            participant.FullName = employee.FullName;
                            //participant.ProfilePicture = employee.ProfilePicture;
                            participants.Add(participant);
                        }

                    }
                }

                Participant my = participants.Where(p => p.EmployeeNumber == employeeNumber).First();
                participants.Remove(my);
                return participants;
            }
            else
            {
                return null;
            }
        }

        public HomeModel ShowAllBenefits(EmployeeModel employeeModel)
        {
            try
            {
                HomeModel homeModel = new HomeModel();
                Dictionary<string, string> BenefitConditions = new Dictionary<string, string>();
                Dictionary<string, bool> BenefitApplicale = new Dictionary<string, bool>();
                List<string> BenefitWorkflows = new List<string>();

                List<BenefitModel> AllBenefitModels = new List<BenefitModel>();
                List<BenefitAPIModel> AllBenefitAPIModels = new List<BenefitAPIModel>();
                List<BenefitAPIModel> AvailableBenefitAPIModels = new List<BenefitAPIModel>();
                BenefitAPIModel benefitAPIModel = new BenefitAPIModel();
                LoginUser User = new LoginUser();
                List<BenefitModel> benefitModels = GetAllBenefits().Result;
                AllBenefitModels = BenefitsUserCanRedeem(benefitModels, employeeModel);

                var requests = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
                if (requests.Count != 0)
                {
                    employeeModel.hasRequests = true;
                    employeeModel.PendingRequestsCount = requests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Pending).Count();

                }
                else
                {
                    employeeModel.hasRequests = false;
                    employeeModel.PendingRequestsCount = 0;

                }
                User = _employeeService.CreateLoginUser(employeeModel);
                if (User != null)
                {
                    homeModel.user = User;
                }
                else
                {
                    homeModel.user = new LoginUser();
                }
                foreach (BenefitModel benefitModel in AllBenefitModels)
                {
                    BenefitConditionsAndAvailable benefitConditionsAndAvailable = CreateBenefitConditions(benefitModel, employeeModel);
                    //BenefitConditions = benefitConditionsAndAvailable.BenefitConditions;
                    //BenefitApplicale = benefitConditionsAndAvailable.BenefitApplicable;
                    benefitModel.BenefitConditions = benefitConditionsAndAvailable.BenefitConditions;
                    benefitModel.BenefitApplicable = benefitConditionsAndAvailable.BenefitApplicable;

                    //if (BenefitConditions != null)
                    //{
                    //    benefitModel.BenefitConditions = BenefitConditions;
                    //}
                    //else
                    //{
                    //    benefitModel.BenefitConditions = new Dictionary<string, string>();
                    //}
                    //if (BenefitApplicale != null)
                    //{
                    //    benefitModel.BenefitApplicable = BenefitApplicale;
                    //}
                    //else
                    //{
                    //    benefitModel.BenefitApplicable = new Dictionary<string, bool>();
                    //}
                    if (benefitModel.RequiredDocuments != null)
                    {
                        benefitModel.RequiredDocumentsArray = benefitModel.RequiredDocuments.Split(";");
                    }
                    BenefitWorkflows = _benefitWorkflowService.CreateBenefitWorkFlow(benefitModel);
                    if (BenefitWorkflows != null)
                    {
                        benefitModel.BenefitWorkflows = BenefitWorkflows;
                    }
                    else
                    {
                        benefitModel.BenefitWorkflows = new List<string>();
                    }
                }
                if (AllBenefitModels != null)
                {
                    foreach (BenefitModel benefitModel in AllBenefitModels)
                    {
                        benefitAPIModel = CreateBenefitAPIModel(benefitModel);
                        if (benefitAPIModel != null)
                        {
                            benefitAPIModel.HasHoldingRequests = _benefitRequestService.ISEmployeeHasHoldingRequestsForthisBenefit(employeeModel.EmployeeNumber, benefitModel.Id);


                            AllBenefitAPIModels.Add(benefitAPIModel);
                        }
                    }
                    var AvailableBenefitModels = AllBenefitAPIModels.Where(b => b.EmployeeCanRedeem == true);
                    foreach (var benefit in AvailableBenefitModels)
                    {
                        benefit.TimesEmployeeReceiveThisBenefit = _benefitRequestService.GetTimesEmployeeReceieveThisBenefit(employeeModel.EmployeeNumber, benefit.Id);
                    }
                    if (AvailableBenefitModels != null)
                    {
                        homeModel.AvailableBenefitModels = AvailableBenefitModels.ToList(); ;

                    }
                    else
                    {
                        homeModel.AvailableBenefitModels = AvailableBenefitAPIModels;

                    }
                    homeModel.AllBenefitModels = AllBenefitAPIModels;

                }
                else
                {
                    homeModel.AllBenefitModels = new List<BenefitAPIModel>();

                }
                return homeModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }

        public BenefitAPIModel GetBenefitDetails(long benefitId, EmployeeModel employeeModel)
        {
            try
            {
                Dictionary<string, string> BenefitConditions = new Dictionary<string, string>();
                Dictionary<string, bool> BenefitApplicale = new Dictionary<string, bool>();
                BenefitModel benefitModel = GetBenefit(benefitId);
                BenefitConditionsAndAvailable benefitConditionsAndAvailable = CreateBenefitConditions(benefitModel, employeeModel);
                benefitModel.BenefitConditions = benefitConditionsAndAvailable.BenefitConditions;
                benefitModel.BenefitApplicable = benefitConditionsAndAvailable.BenefitApplicable;
                //if (BenefitConditions != null)
                //{
                //    benefitModel.BenefitConditions = BenefitConditions;
                //}
                //else
                //{
                //    benefitModel.BenefitConditions = new Dictionary<string, string>();
                //}
                //if (BenefitApplicale != null)
                //{
                //    benefitModel.BenefitApplicable = BenefitApplicale;
                //}
                //else
                //{
                //    benefitModel.BenefitApplicable = new Dictionary<string, bool>();
                //}

                benefitModel.BenefitWorkflows = _benefitWorkflowService.CreateBenefitWorkFlow(benefitModel);
                List<BenefitModel> benefitModels = new List<BenefitModel>();
                benefitModels.Add(benefitModel);
                benefitModels = BenefitsUserCanRedeem(benefitModels, employeeModel);
                benefitModel = benefitModels.First();
                BenefitAPIModel benefitAPIModel = CreateBenefitAPIModel(benefitModel);
                return benefitAPIModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<Participant> GetEmployeesWhoCanIGiveThisBenefit(long employeeNumber, long benefitId)
        {
            EmployeeModel employeeModel = _employeeService.GetEmployee(employeeNumber);
            List<Participant> participants = new List<Participant>();

            if (employeeModel != null)
            {
                BenefitModel benefitModel = GetBenefit(benefitId);
                if (benefitModel.IsAgift == true)
                {
                    var employeeModels = _employeeService.GetAllEmployeeWhoCanIGive().Result;
                    if (employeeModels != null)
                    {
                        List<EmployeeModel> employeeModels1 = employeeModels.ToList();
                        foreach (var employee in employeeModels)
                        {
                            Participant participant = new Participant();
                            participant.EmployeeNumber = employee.EmployeeNumber;
                            participant.FullName = employee.FullName;
                            participant.ProfilePicture = employee.ProfilePicture;
                            participants.Add(participant);
                        }
                        var my = participants.Where(p => p.EmployeeNumber == employeeNumber).First();
                        participants.Remove(my);
                        return participants;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public List<BenefitStats> GetIndividualBenefitStats(List<BenefitRequestModel> benefitRequests)
        {
            int pendingCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Pending).Count();
            int InProgressCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.InProgress).Count();
            int ApprovedCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Approved).Count();
            int RejectedCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Rejected).Count();
            int CancelledCount = benefitRequests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Cancelled).Count();
            int Closed = ApprovedCount + RejectedCount + CancelledCount;

            List<BenefitStats> benefitStatses = new List<BenefitStats>(8);
            for (int index = 1; index <= 5; index++)
            {
                BenefitStats benefitStats = new BenefitStats();
                benefitStats.Name = Enum.GetName(typeof(CommanData.BenefitStatus), index);
                benefitStats.Count = benefitRequests.Where(r => r.RequestStatusId == index).Count();
                benefitStatses.Add(benefitStats);
            }

            BenefitStats benefitStats1 = new BenefitStats();
            benefitStats1.Name = "Closed";
            benefitStats1.Count = benefitStatses.Where(s => s.Name == "Cancelled" || s.Name == "Approved" || s.Name == "Rejected").Select(s => s.Count).Sum();
            benefitStatses.Add(benefitStats1);
            BenefitStats benefitStats2 = new BenefitStats();
            benefitStats2.Name = "ALL";
            benefitStats2.Count = benefitStatses.Where(s => s.Name == "Cancelled"
                                                         || s.Name == "Approved"
                                                         || s.Name == "Rejected"
                                                         || s.Name == "Pending"
                                                         || s.Name == "InProgress").Select(s => s.Count).Sum();
            benefitStatses.Add(benefitStats2);

            return benefitStatses;
        }


        public List<BenefitStats> GetGroupBenefitStats(List<GroupEmployeeModel> groupEmployeeModels)
        {

            List<BenefitStats> benefitStatses = new List<BenefitStats>(8);
            for (int index = 1; index <= 5; index++)
            {
                BenefitStats benefitStats = new BenefitStats();
                benefitStats.Name = Enum.GetName(typeof(CommanData.BenefitStatus), index);
                benefitStats.Count = groupEmployeeModels.Where(gr => gr.Group.RequestStatusId == index).Count();
                benefitStatses.Insert(index, benefitStats);
            }

            BenefitStats benefitStats1 = new BenefitStats();
            benefitStats1.Name = "Closed";
            benefitStats1.Count = benefitStatses.Where(s => s.Name == "Cancelled" || s.Name == "Approved" || s.Name == "Rejected").Count();
            benefitStatses.Insert(6, benefitStats1);
            BenefitStats benefitStats2 = new BenefitStats();
            benefitStats2.Name = "ALL";
            benefitStats2.Count = benefitStatses.Where(s => s.Name == "Cancelled"
                                                         || s.Name == "Approved"
                                                         || s.Name == "Rejected"
                                                         || s.Name == "Pending"
                                                         || s.Name == "InProgress").Count();
            benefitStatses.Insert(7, benefitStats2);

            return benefitStatses;
        }


        public Request BenefitRedeem(long benefitId, string userId)
        {
            EmployeeModel CurrentEmployee = _employeeService.GetEmployeeByUserId(userId).Result;
            BenefitModel benefitModel = GetBenefit(benefitId);
            Request request = new Request();
            //benefitModel.BenefitConditions = CreateBenefitConditions(benefitModel);
            benefitModel.EmployeeCanRedeem = true;
            //benefitRequestModel.Benefit = benefitModel;
            if (benefitModel.DateToMatch == "Any")
            {
                request.From = DateTime.Today.ToString("yyyy-MM-dd");
                request.To = DateTime.Today.AddDays(benefitModel.numberOfDays - 1).ToString("yyyy-MM-dd");
            }
            else if (benefitModel.DateToMatch != "Any")
            {
                if (benefitModel.DateToMatch == "Birth Date")
                {
                    request.From = new DateTime(DateTime.Today.Year, CurrentEmployee.BirthDate.Month, CurrentEmployee.BirthDate.Day).ToString("yyyy-MM-dd");
                    request.To = new DateTime(DateTime.Today.Year, CurrentEmployee.BirthDate.Month, CurrentEmployee.BirthDate.Day).ToString("yyyy-MM-dd");
                }
                else if (benefitModel.DateToMatch == "Join Date")
                {
                    request.From = new DateTime(DateTime.Today.Year, CurrentEmployee.JoiningDate.Month, CurrentEmployee.JoiningDate.Day).ToString("yyyy-MM-dd");
                    request.To = new DateTime(DateTime.Today.Year, CurrentEmployee.JoiningDate.Month, CurrentEmployee.JoiningDate.Day).ToString("yyyy-MM-dd");
                }
                else
                {
                    request.From = new DateTime(DateTime.Today.Year, benefitModel.CertainDate.Month, benefitModel.CertainDate.Day).ToString("yyyy-MM-dd");
                    request.To = new DateTime(DateTime.Today.Year, benefitModel.CertainDate.Month, benefitModel.CertainDate.Day).ToString("yyyy-MM-dd");
                }
            }
            request.numberOfDays = benefitModel.numberOfDays;
            request.DateToMatch = benefitModel.DateToMatch;
            request.Year = benefitModel.Year;
            request.BenefitName = benefitModel.Name;
            request.IsAgift = benefitModel.IsAgift;
            request.benefitId = benefitModel.Id;
            if (benefitModel.RequiredDocuments != null)
            {
                request.RequiredDocuments = benefitModel.RequiredDocuments.Split(";");
            }
            request.BenefitType = benefitModel.BenefitType.Name;
            if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
            {
                return request;
            }
            else
            {
                request.MaxParticipant = benefitModel.MaxParticipant;
                request.MinParticipant = benefitModel.MinParticipant;
                request.ParticipantsData = GetEmployeesCanRedeemThisGroupBenefit(CurrentEmployee.EmployeeNumber, benefitId);
                return request;
            }
        }
    }
}