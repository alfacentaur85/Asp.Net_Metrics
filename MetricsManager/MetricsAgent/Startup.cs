using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL;
using System.Data.SQLite;
using System.IO;
using MetricsAgent.DAL.Interfaces;
using AutoMapper;
using Dapper;
<<<<<<< HEAD
=======
using FluentMigrator.Runner;
using Quartz;
using Quartz.Spi;
using MetricsAgent.Factories;
using MetricsAgent.Jobs;
using MetricsAgent.DTO;
using Quartz.Impl;
>>>>>>> Lesson5

namespace MetricsAgent
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
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
<<<<<<< HEAD
=======

>>>>>>> Lesson5
            services.AddControllers();

<<<<<<< HEAD
        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            PrepareSchema(connection);
        }
=======
            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetWorkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
>>>>>>> Lesson5

            // ДОбавляем сервисы
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();


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


<<<<<<< HEAD
                    command.CommandText = string.Concat("CREATE TABLE ", metricsType," (id INTEGER PRIMARY KEY,value INT, time INT)");
                    command.ExecuteNonQuery();
                }            
                
            }
=======
>>>>>>> Lesson5
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            // запускаем миграции
            migrationRunner.MigrateUp();


        }
    }
}
