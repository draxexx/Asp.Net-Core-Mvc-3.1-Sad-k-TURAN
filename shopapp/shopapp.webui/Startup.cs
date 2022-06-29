using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.data.EfCore;
using shopapp.webui.EmailServices;
using shopapp.webui.Identity;

namespace shopapp.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {   
            services.AddDbContext<ApplicationContext>(options=>options.UseSqlite("Data Source=shopDb"));
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            //identity ayarları
            services.Configure<IdentityOptions>(options=> {
                options.Password.RequireDigit = true; //parola içinde mutlaka sayısal değer girmesi gerekiyor
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true; //özel karakter olmak zorunda

                options.Lockout.MaxFailedAccessAttempts = 5; //maksimum 5 defa yanlış parola girebilir
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //kilitlendikten 5 dakika sonra açılmasına izin veririz
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true; //hesabını onaylaması gerekiyor
                options.SignIn.RequireConfirmedPhoneNumber = false; //telefonu için onaylaması gerekiyor
            });

            //kullanıcı tarayıcısına uygulama tarafından bırakılan bilgiler
            services.ConfigureApplicationCookie(options=>{
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //login olduktan sonra 60 dakika tanır
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true, //sadece http talebi alır
                    Name = ".ShopApp.Security.Cookie", //cookie name
                    SameSite = SameSiteMode.Strict,
                };
            });

            //hangi repository kullanmak istiyorsak buradan tanımlamalıyız, MySQL, SQL, Oracle...
            services.AddScoped<IProductRepository,EfCoreProductRepository>();
            services.AddScoped<IProductService,ProductManager>();
            
            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();
            services.AddScoped<ICategoryService,CategoryManager>();

            services.AddScoped<ICartRepository,EfCoreCartRepository>();
            services.AddScoped<ICartService,CartManager>();

            services.AddScoped<IOrderRepository,EfCoreOrderRepository>();
            services.AddScoped<IOrderService,OrderManager>();

            services.AddScoped<IEmailSender,SmtpEmailSender>(i=>
                new SmtpEmailSender(
                    _configuration["EmailSender:Host"],
                    _configuration.GetValue<int>("EmailSender:Port"),
                    _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    _configuration["EmailSender:UserName"],
                    _configuration["EmailSender:Password"]
                    ));

            //mvc yapısını kullanacağımızı belirtiyoruz
            //razor pages
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles(); //wwwroot kullanabiliriz

            //nodejs yükledikten sonra bilgisayara, proje içerisine eklediğim node_modules kullanmak için tanımlıyoruz wwwroot gibi
            app.UseStaticFiles(new StaticFileOptions{
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")
                ),
                RequestPath="/modules"
            });

            //uygulama geliştirilirken bu if çalışır
            if (env.IsDevelopment())
            {
                SeedDatabase.Seed(); //test verilerini ekleme işlemini tetikleriz
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            //localhost:5000
            //localhost:5000/products
            //localhost:5000/products/5
            app.UseEndpoints(endpoints =>
            {  
                endpoints.MapControllerRoute(
                    name: "orders",
                    pattern: "orders",
                    defaults: new {controller="Cart", action="GetOrders"}
                );

                endpoints.MapControllerRoute(
                    name: "checkout",
                    pattern: "checkout",
                    defaults: new {controller="Cart", action="Checkout"}
                );

                endpoints.MapControllerRoute(
                    name: "cart",
                    pattern: "cart",
                    defaults: new {controller="Cart", action="Index"}
                );

                endpoints.MapControllerRoute(
                    name: "adminuseredit",
                    pattern: "admin/user/{id?}",
                    defaults: new {controller="Admin", action="UserEdit"}
                );

                endpoints.MapControllerRoute(
                    name: "adminusers",
                    pattern: "admin/user/list",
                    defaults: new {controller="Admin", action="UserList"}
                ); 

                endpoints.MapControllerRoute(
                    name: "adminroles",
                    pattern: "admin/role/list",
                    defaults: new {controller="Admin", action="RoleList"}
                );

                endpoints.MapControllerRoute(
                    name: "adminrolecreate",
                    pattern: "admin/role/create",
                    defaults: new {controller="Admin", action="RoleCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "adminroleedit",
                    pattern: "admin/role/{id?}",
                    defaults: new {controller="Admin", action="RoleEdit"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new {controller="Admin", action="ProductList"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductcreate",
                    pattern: "admin/products/create",
                    defaults: new {controller="Admin", action="ProductCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductedit",
                    pattern: "admin/products/{id?}",
                    defaults: new {controller="Admin", action="ProductEdit"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategories",
                    pattern: "admin/categories",
                    defaults: new {controller="Admin", action="CategoryList"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategorycreate",
                    pattern: "admin/categories/create",
                    defaults: new {controller="Admin", action="CategoryCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategoryedit",
                    pattern: "admin/categories/{id?}",
                    defaults: new {controller="Admin", action="CategoryEdit"}
                );

                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new {controller="Shop", action="Search"}
                );

                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new {controller="Shop", action="Details"}
                );

                //shop/list giderken url /products olarak gelir
                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new {controller="Shop", action="List"}
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        
            SeedIdentity.Seed(userManager,roleManager,configuration).Wait();
        }
    }
}
