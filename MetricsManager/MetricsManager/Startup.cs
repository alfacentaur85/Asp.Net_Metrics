using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using FluentMigrator.Runner;
using Quartz.Impl;
using Quartz;
using Quartz.Spi;
using System.Data.SQLite;
using MetricsManager.Jobs;
using MetricsManager.Factories;
using MetricsManager.DTO;
using Quartz.Core;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Client;
using Polly;
using System;
using AutoMapper;
using MetricsManager.DAL.Repositories;
using MetricsManager.DAL.Models;
using System.Collections.Generic;


namespace MetricsManager
{
    public class Startup
    {
        public static string ConnectionString
        {
            get
            {
                using (StreamReader sr = new StreamReader("ConnectionString.txt"))
                {
                    return sr.ReadLine();
                }
            }
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ДОбавляем сервисы
            services.AddSingleton<IJobFactory, SingletonJobFactory>();

            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();

            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();

            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();

            services.AddSingleton<INetworkMetricsRepository, NetWorkMetricsRepository>();

            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

            services.AddSingleton<IAgentsRepositorySingle, AgentsRepository>();

            services.AddFluentMigratorCore()
              .ConfigureRunner(rb => rb
                  // добавляем поддержку SQLite 
                  .AddSQLite()
                  // устанавливаем строку подключения
                  .WithGlobalConnectionString(ConnectionString)
                  // подсказываем где искать классы с миграциями
                  .ScanIn(typeof(Startup).Assembly).For.Migrations()
              ).AddLogging(lb => lb
                  .AddFluentMigratorConsole());

            services.AddSingleton<CpuMetricJob>();

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<RamMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<HddMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<DotNetMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: "0/5 * * * * ?"));
            
            services.AddHostedService<QuartzHostedService>();

            services.AddControllers();

            services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
               .AddTransientHttpErrorPolicy(p =>
               p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner, IAgentsRepositorySingle agentsRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // запускаем миграции
            migrationRunner.MigrateUp();
  
            var AgentsJob = new AgentsJob(agentsRepository);
            AgentsJob.Delete();
            AgentsJob.Execute(null);

        }




    }
}
