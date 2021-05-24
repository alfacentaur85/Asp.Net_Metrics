using MetricsAgent.Responses;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Core;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;

namespace MetricsAgent.DAL
{
    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке


    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        public DotNetMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffSetHandler());
        }

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public void Create(DotNetMetric item)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            connection.Execute(string.Concat("INSERT INTO ", MetricsType.metricsList[(int)MetricsTypeEnum.DotNetMetrics], "(value, time) VALUES(@value, @time)"),
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DotNetMetrics], " WHERE id=@id"),
               new
               {
                   id = id
               });
        }

        public DotNetMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            return connection.QuerySingle<DotNetMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DotNetMetrics])
                    ),
                    new
                    {
                        id = id
                    }
                );

        }

        public void Update(DotNetMetric item)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("UPDATE ", MetricsType.metricsList[(int)MetricsTypeEnum.DotNetMetrics], " SET value = @value, time = @time WHERE id=@id"),
               new
               {
                   id = item.Id,
                   value = item.Value,
                   time = item.Time.ToUnixTimeSeconds()
               });
        }

        public IList<DotNetMetric> GetAll()
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            return connection.Query<DotNetMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DotNetMetrics])
                    )
                  ).AsList<DotNetMetric>();
        }

        public IList<DotNetMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            return connection.Query<DotNetMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DotNetMetrics], " WHERE time>=@from and time<=@to")
                    ),

                    new
                    {
                        from = fromTime.ToUnixTimeSeconds(),
                        to = toTime.ToUnixTimeSeconds()
                    }
                ).AsList<DotNetMetric>();
        }
    }
}
