using MaxEndLabs.Data;
using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.Services.Core;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.Services.Core.Models.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;
using ProductService = MaxEndLabs.Services.Core.ProductService;


namespace MaxEndLabs.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

            builder.Services.AddDbContext<MaxEndLabsDbContext>(options =>
            {
                // This dynamically gets the name of your Data project (MaxEndLabs.Data)
                var migrationsAssembly = typeof(MaxEndLabsDbContext).Assembly.FullName;

                if (dbProvider == "PostgreSQL")
                {
                    var postgresConn = builder.Configuration.GetConnectionString("PostgresConnection")
                                       ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");

                    options.UseNpgsql(postgresConn, b => b.MigrationsAssembly(migrationsAssembly));
                }
                else
                {
                    var sqlConn = builder.Configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                    options.UseSqlServer(sqlConn, b => b.MigrationsAssembly(migrationsAssembly));
                }
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<MaxEndLabsDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

			builder.Services.AddRazorPages();

			builder.Services.Configure<GoogleReCaptchaSettings>(
				builder.Configuration.GetSection("GoogleReCaptcha"));
			builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddHttpClient<IReCaptchaService, ReCaptchaService>();
            builder.Services.AddScoped<IStripeService,StripeService>();

			builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

			builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

			builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MaxEndLabsDbContext>();
                var currentProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

                if (currentProvider == "PostgreSQL")
                {
                    // Render.com:
                    dbContext.Database.Migrate();
                }
                else
                {
                    dbContext.Database.EnsureCreated();
                }
            }

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

			app.Run();
        }
    }
}
