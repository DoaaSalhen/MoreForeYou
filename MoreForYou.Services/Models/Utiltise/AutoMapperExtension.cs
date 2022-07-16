using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;

namespace MoreForYou.Services.Models.Utilities.Mapping
{
    public static class AutoMapperExtension
    {
        public static void ConfigAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AssembleType));
        }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Employee, EmployeeModel>();
                CreateMap<EmployeeModel, Employee>();
                CreateMap<Department, DepartmentModel>();
                CreateMap<DepartmentModel, Department>();
                CreateMap<Position, PositionModel>();
                CreateMap<PositionModel, Position>();
                CreateMap<Nationality, NationalityModel>();
                CreateMap<NationalityModel, Nationality>();
                CreateMap<UserModel, AspNetUser>();
                CreateMap<AspNetUser, UserModel>();
                CreateMap<RoleModel, IdentityRole>();
                CreateMap<IdentityRole, RoleModel>();
                CreateMap<Benefit, BenefitModel>();
                CreateMap<BenefitModel, Benefit>();
                CreateMap<BenefitType, BenefitTypeModel>();
                CreateMap<BenefitTypeModel, BenefitType>();
                CreateMap<BenefitWorkflow, BenefitWorkflowModel>();
                CreateMap<BenefitWorkflowModel, BenefitWorkflow>();
                CreateMap<BenefitRequest, BenefitRequestModel>();
                CreateMap<BenefitRequestModel, BenefitRequest>();
                CreateMap<RequestWorkflow, RequestWokflowModel>();
                CreateMap<RequestWokflowModel, RequestWorkflow>();
                CreateMap<RequestStatus, RequestStatusModel>();
                CreateMap<RequestStatusModel, RequestStatus>();
                //CreateMap<EmployeeRequest, EmployeeRequestModel>();
                //CreateMap<EmployeeRequestModel, EmployeeRequest>();
                CreateMap<Group, GroupModel>();
                CreateMap<GroupModel, Group>();
                CreateMap<Company, CompanyModel>();
                CreateMap<CompanyModel, Company>();
                CreateMap<GroupEmployee, GroupEmployeeModel>();
                CreateMap<GroupEmployeeModel, GroupEmployee>();
                CreateMap<Notification, NotificationModel>();
                CreateMap<NotificationModel, Notification>();
                CreateMap<UserNotification, UserNotificationModel>();
                CreateMap<UserNotificationModel, UserNotification>();
                CreateMap<Privilege, PrivilegeModel>();
                CreateMap<PrivilegeModel, Privilege>();

                CreateMap<RequestDocument, RequestDocumentModel>();
                CreateMap<RequestDocumentModel, RequestDocument>();


            }
        }
    }
    public class AssembleType
    {
    }
}