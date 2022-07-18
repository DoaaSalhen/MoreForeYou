using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreForYou.Services.Implementation
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IRepository<UserNotification, long> _repository;
        private readonly ILogger<UserNotificationService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;


        public UserNotificationService(IRepository<UserNotification, long> repository,
            ILogger<UserNotificationService> logger,
            IMapper mapper,
            IEmployeeService employeeService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _employeeService = employeeService;
        }
        public UserNotificationModel CreateUserNotification(UserNotificationModel model)
        {
            try
            {
                UserNotification userNotification = _mapper.Map<UserNotification>(model);
                userNotification = _repository.Add(userNotification);
                UserNotificationModel Newmodel = _mapper.Map<UserNotificationModel>(userNotification);
                return Newmodel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public List<UserNotificationModel> GetUserNotification(string userId)
        {
            try
            {
                List<UserNotification> userNotifications = _repository.Find(UN => UN.Employee.UserId == userId, false, UN => UN.Notification, UN => UN.Notification.BenefitRequest, UN => UN.Notification.BenefitRequest.RequestStatus, UN => UN.Notification.BenefitRequest.Benefit, UN => UN.Notification.BenefitRequest.Employee).ToList();
                List<UserNotificationModel> userNotificationModels = _mapper.Map<List<UserNotificationModel>>(userNotifications);
                if (userNotificationModels != null)
                {
                    if (userNotificationModels.Count > 10)
                    {
                        userNotificationModels = userNotificationModels.TakeLast(10).ToList();
                    }
                    foreach (UserNotificationModel userNotificationModel in userNotificationModels.Where(UN => UN.Notification.Type == "Response"))
                    {
                        userNotificationModel.ResponsedByEmployee = _employeeService.GetEmployee((long)userNotificationModel.Notification.ResponsedBy);
                    }
                }
                return userNotificationModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }

        public List<NotificationAPIModel> CreateNotificationAPIModel(List<UserNotificationModel> userNotificationModels)
        {
            try
            {
                List<NotificationAPIModel> notificationAPIModels = new List<NotificationAPIModel>();

                foreach (var userNotification in userNotificationModels)
                {
                    NotificationAPIModel notificationAPIModel = new NotificationAPIModel();
                    if (userNotification.Notification.Type == "Request" || userNotification.Notification.Type == "CreateGroup")
                    {
                        notificationAPIModel.EmployeeFullName = userNotification.Notification.BenefitRequest.Employee.FullName;
                        notificationAPIModel.EmployeeNumber = userNotification.Notification.BenefitRequest.Employee.EmployeeNumber;
                        //notificationAPIModel.EmployeeProfilePicture = userNotification.Notification.BenefitRequest.Employee.ProfilePicture;

                    }
                    else if (userNotification.Notification.Type == "Response")
                    {
                        EmployeeModel employeeModel = _employeeService.GetEmployee((long)userNotification.Notification.ResponsedBy);
                        notificationAPIModel.EmployeeNumber = employeeModel.EmployeeNumber;
                        notificationAPIModel.EmployeeFullName = employeeModel.FullName;
                        //notificationAPIModel.EmployeeProfilePicture = employeeModel.ProfilePicture;

                    }
                    notificationAPIModel.Message = userNotification.Notification.Message;
                    notificationAPIModel.NotificationType = userNotification.Notification.Type;
                    notificationAPIModel.RequestStatus = userNotification.Notification.BenefitRequest.RequestStatus.Name;
                    notificationAPIModel.Date = userNotification.CreatedDate;
                    notificationAPIModel.BenefitId = userNotification.Notification.BenefitRequest.BenefitId;
                    notificationAPIModel.RequestNumber = userNotification.Notification.BenefitRequestId;
                    notificationAPIModels.Add(notificationAPIModel);
                }
                return notificationAPIModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public List<UserNotificationModel> GetFiftyUserNotification(string userId)
        {
            try
            {
                List<UserNotification> userNotifications = _repository.Find(UN => UN.Employee.UserId == userId, false, UN => UN.Notification, UN => UN.Notification.BenefitRequest, UN => UN.Notification.BenefitRequest.RequestStatus, UN => UN.Notification.BenefitRequest.Benefit, UN => UN.Notification.BenefitRequest.Employee).ToList();
                List<UserNotificationModel> userNotificationModels = _mapper.Map<List<UserNotificationModel>>(userNotifications);
                if (userNotificationModels != null)
                {
                    if (userNotificationModels.Count > 50)
                    {
                        userNotificationModels = userNotificationModels.TakeLast(50).ToList();
                    }
                    //var ResponseNotifications = userNotificationModels.Where(UN => UN.Notification.Type == "Response");
                    //if(ResponseNotifications.Count() > 0)
                    //{
                    //    foreach (UserNotificationModel userNotificationModel in ResponseNotifications)
                    //    {
                    //        userNotificationModel.ResponsedByEmployee = _employeeService.GetEmployee((long)userNotificationModel.Notification.ResponsedBy);
                    //    }
                    //}
                   
                }
                return userNotificationModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
    }
}
