using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medtracker.Infrastructure;
using medtracker.SQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace medtracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(Configuration);
            services.AddSingleton<CredentialsRepository>();
            services.AddSingleton<ISubscriberRepository, SubscriberRepository>();
            services.AddSingleton<IUserTimesRepository, UserTimesRepository>();
            services.AddSingleton<IDataRepository, DataRepository>();
            services.AddSingleton<CommandHandler>();
            services.AddSingleton<UserAlertService>();
            services.AddSingleton<UserRecordService>();
            services.AddTransient<SlackAPI>();
            services.AddSingleton<IHostedService, BackgroundAlertService>();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CredentialsRepository>();
                context.CreateTableIfNotExists();
                var userTimeContext = serviceScope.ServiceProvider.GetRequiredService<IUserTimesRepository>();
                userTimeContext.CreateTableIfNotExists();
                var dataContext = serviceScope.ServiceProvider.GetRequiredService<IDataRepository>();
                dataContext.CreateTableIfNotExists();
                var subscriberContext = serviceScope.ServiceProvider.GetRequiredService<ISubscriberRepository>();
                subscriberContext.CreateTableIfNotExists();
            }
        }
    }
}
