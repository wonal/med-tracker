using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            services.AddSingleton<UserTimesRepository>();
            services.AddSingleton<CommandHandler>();
            services.AddTransient<SlackAPI>();
            services.AddSingleton<IHostedService, BackgroundService>();
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
                var userTimeContext = serviceScope.ServiceProvider.GetRequiredService<UserTimesRepository>();
                userTimeContext.CreateTableIfNotExists();
            }
        }
    }
}
