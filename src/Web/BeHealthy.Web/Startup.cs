namespace BeHealthy.Web
{
    using System.Reflection;

    using BeHealthy.Data;
    using BeHealthy.Data.Common;
    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Data.Repositories;
    using BeHealthy.Data.Seeding;
    using BeHealthy.Services.Cloudinary;
    using BeHealthy.Services.Data.Exercises;
    using BeHealthy.Services.Data.ExerciseSteps;
    using BeHealthy.Services.Data.Ratings;
    using BeHealthy.Services.Data.Tags;
    using BeHealthy.Services.Data.Users;
    using BeHealthy.Services.Data.WorkoutExercises;
    using BeHealthy.Services.Data.Workouts;
    using BeHealthy.Services.Mapping;
    using BeHealthy.Services.Messaging;
    using BeHealthy.Web.Dtos;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.Strict;
                    });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews(configure =>
            {
                configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "CSRF-TOKEN";
            });
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IExercisesService, ExercisesService>();
            services.AddTransient<IExerciseStepsService, ExerciseStepsService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IRatingsService, RatingsService>();
            services.AddTransient<IWorkoutsService, WorkoutsService>();
            services.AddTransient<IWorkoutExercisesService, WorkoutExercisesService>();
            services.AddTransient<IUsersService, UsersService>();

            var sendGridEmailSender = new SendGridEmailSender(
                this.configuration["SendGrid:ApiKey"],
                this.configuration["SendGrid:SenderEmail"],
                this.configuration["SendGrid:SenderName"]);
            services.AddTransient<IEmailSender, SendGridEmailSender>(x => sendGridEmailSender);

            var account = new Account
            {
                Cloud = this.configuration["Cloudinary:CloudName"],
                ApiKey = this.configuration["Cloudinary:ApiKey"],
                ApiSecret = this.configuration["Cloudinary:ApiSecret"],
            };
            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);
            services.AddTransient<ICloudinaryService, CloudinaryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
