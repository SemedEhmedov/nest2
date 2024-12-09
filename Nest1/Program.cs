using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nest1.Abstrcations.emailservice;
using Nest1.DAL;
using Nest1.Helpers.email;
using Nest1.Models;
using Nest1.Services;
using System;
using System.Configuration;

namespace Nest1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IMailService,MailService>();
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;     
                opt.Password.RequiredLength = 8;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            }).AddEntityFrameworkStores<AppDBContext>(); 
            builder.Services.AddDbContext<AppDBContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("default"));
            });
            var app = builder.Build();




            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=DashBoard}/{action=index}/{id?}"
               );
            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}"
                 );
            app.UseStaticFiles();
            app.Run();
        }
    }
}
