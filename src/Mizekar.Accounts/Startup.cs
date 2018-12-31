using System;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mizekar.Accounts.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mizekar.Accounts.Data.Entities;
using Mizekar.Accounts.Services;
using Mizekar.Accounts.Validation;

namespace Mizekar.Accounts
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsService, SmsService>();

            var dbConnection = Configuration["db_connection"];
            services.AddDbContext<AccountsDbContext>(options =>
                options.UseSqlServer(dbConnection));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<AccountsDbContext>()
                .AddDefaultTokenProviders();

            services.AddCors(o => o.AddPolicy("MyPolicy",
                corsBuilder =>
                {
                    corsBuilder
                        //.WithOrigins("http://88.99.110.152:62302", "http://88.99.110.152:62303", "http://localhost:4200", "http://localhost:62302", "http://localhost:62303")
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithExposedHeaders("orgid");
                })
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure identity server
            var publicOrigin = Configuration["public_origin"];
            var dbConnectionIdsConfig = Configuration["db_connection_ids_config"];
            var dbConnectionIdsOperations = Configuration["db_connection_ids_operations"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var builder = services.AddIdentityServer(options =>
            {
                options.PublicOrigin = publicOrigin;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            });

            builder.AddAspNetIdentity<ApplicationUser>();
                //.AddProfileService<ProfileService>();

            builder.AddConfigurationStore<ConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = builder1 =>
                    builder1.UseSqlServer(dbConnectionIdsConfig,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            });

            // this adds the operational data from DB (codes, tokens, consents)
            builder.AddOperationalStore<PersistedGrantDbContext>(options =>
            {
                options.ConfigureDbContext = builder2 =>
                    builder2.UseSqlServer(dbConnectionIdsOperations,
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });
            builder.AddExtensionGrantValidator<PhoneNumberTokenGrantValidator>();

            //if (Environment.IsDevelopment())
            //{
            builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    throw new Exception("need to configure key material");
            //}

            //services.AddAuthentication()
            //  .AddGoogle(options =>
            //  {
            //      options.ClientId = ;
            //      options.ClientSecret = ;
            //  });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DatabaseMigrate.InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");

            //app.UseCookiePolicy();

            // app.UseAuthentication(); // not needed, since UseIdentityServer adds the authentication middleware
            app.UseIdentityServer(); ;

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AccountsDbContext>();
                if (!context.Database.IsInMemory())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}