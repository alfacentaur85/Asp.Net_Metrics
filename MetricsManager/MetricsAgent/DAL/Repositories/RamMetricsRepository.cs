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


    public class RamMetricsRepository : IRamMetricsRepository
    {
        public RamMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffSetHandler());
        }

        // инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(RamMetric item)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            connection.Execute(string.Concat("INSERT INTO ", MetricsType.metricsList[(int)MetricsTypeEnum.RamMetrics], "(value, time) VALUES(@value, @time)"),
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.RamMetrics], " WHERE id=@id"),
               new
               {
                   id = id
               });
        }

        public void Update(RamMetric item)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("UPDATE ", MetricsType.metricsList[(int)MetricsTypeEnum.RamMetrics], " SET value = @value, time = @time WHERE id=@id"),
               new
               {
                   id = item.Id,
                   value = item.Value,
                   time = item.Time.ToUnixTimeSeconds()
               });
        }

        public IList<RamMetric> GetAll()
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            return connection.Query<RamMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.RamMetrics])
                    )
                  ).AsList<RamMetric>();
        }

        public RamMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            return connection.QuerySingle<RamMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.RamMetrics])
                    ),

                    new
                    {
                        id = id
                    }
                );

        }

        public IList<RamMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            return connection.Query<RamMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.RamMetrics], " WHERE time>=@from and time<=@to")
                    ),

                    new
                    {
                        from = fromTime.ToUnixTimeSeconds(),
                        to = toTime.ToUnixTimeSeconds()
                    }
                ).AsList<RamMetric>();
        }
    }
}
