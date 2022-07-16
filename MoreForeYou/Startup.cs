using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.EntityFramework;
using MoreForYou.Data;
using Microsoft.AspNetCore.Identity;
using MoreForYou.Services.Models.Utilities.Mapping;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Implementation;
using Data.Repository;
using MoreForYou.Models.Models;
using MoreForYou.Service.Implementation.Auth;
using MoreForYou.Service.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using MoreForYou.Controllers.hub;

namespace MoreForeYou
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<APPDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("SqlCon")));

            var connectionString = Configuration.GetConnectionString("SqlCon");
            services.AddDbContext<AuthDBContext>(options =>
            {
                options.UseSqlServer(connectionString,
                 b => b.MigrationsAssembly(typeof(AuthDBContext).Assembly.FullName));
            }, ServiceLifetime.Transient);

            services.AddIdentity<AspNetUser, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;

            })
            .AddEntityFrameworkStores<AuthDBContext>()
            .AddRoles<IdentityRole>()
            .AddDefaultUI().AddDefaultTokenProviders();
            services.AddScoped<DbContext, APPDBContext>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<UserManager<AspNetUser>>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IBenefitService, BenefitService>();
            services.AddScoped<IBenefitTypeService, BenefitTypeService>();
            services.AddScoped<IBenefitWorkflowService, BenefitWorkflowService>();
            services.AddScoped<IBenefitRequestService, BenefitRequestService>();
            services.AddScoped<IRequestWorkflowService, RequestWorkflowService>();
            services.AddScoped<IRequestStatusService, RequestStatusService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IEmployeeRequestService, EmployeeRequestService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGroupEmployeeService, GroupEmployeeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IUserConnectionManager, UserConnectionManager>();
            services.AddScoped<IPrivilegeService, PrivilegeService>();
            services.AddScoped<IRequestDocumentService, RequestDocumentService>();
            services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();



            services.ConfigAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MoreForYou", Version = "v1" });
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc(option =>
            {
                option.CacheProfiles.Add("Default30",
                    new CacheProfile()
                    {
                        Duration = 30
                    });
            });
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://20.86.97.165/more4u")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddSignalR();
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddMvc().AddMvcOptions(options =>
            {
                options.MaxModelValidationErrors = 999999;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoreForYou.API");
                c.EnableFilter();
            });
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{more4u}/{controller=Benefit}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
                endpoints.MapHub<NotificationHub>("/NotificationHub");

            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
