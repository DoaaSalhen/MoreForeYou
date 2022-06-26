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
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification, long> _repository;

        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;


        public NotificationService(IRepository<Notification, long> repository,
            ILogger<NotificationService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public NotificationModel CreateNotification(NotificationModel model)
        {
            try
            {
                Notification Notification = _mapper.Map<Notification>(model);
                Notification = _repository.Add(Notification);
                NotificationModel Newmodel = _mapper.Map<NotificationModel>(Notification);
                return Newmodel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public List<NotificationModel> GetAllUnseenNotificationsForEmployee(long EmployeeNumber)
        {
            throw new NotImplementedException();
        }

        public NotificationModel GetNotification(int id)
        {
            throw new NotImplementedException();
        }

        public List<NotificationModel> GetUnseenNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNotification(NotificationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
