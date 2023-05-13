using Pronia.Database;
using Microsoft.EntityFrameworkCore;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDb>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseServer"));
            }
            );

            builder.Services.AddControllersWithViews();
           
            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );

            app.Run();
        }
    }
}