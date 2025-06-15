namespace PersonalPortfolio.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.Cookie.Name = "admin_auth_cookie";
                    options.LoginPath = "/admin/login"; // Giri� sayfas�
                    options.AccessDeniedPath = "/admin/unauthorized";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // S�reyi burada belirliyorsun
                    options.SlidingExpiration = false; // Her istekle s�re uzas�n m�?
                });

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // 1. User Area'y� area ad� G�R�NMEDEN kullan
            app.MapControllerRoute(
                name: "user_area_without_area_prefix",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "User" });

            // 2. Di�er area'lar normal g�r�ns�n (�rn: Admin)
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
