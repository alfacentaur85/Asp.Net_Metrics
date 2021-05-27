using MetricsManager.Responses;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Core;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DTO;

namespace MetricsManager.DAL.Repositories
{
    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке


    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        public CpuMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffSetHandler());
        }

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public DateTimeOffset GetMaxDate(int AgentId)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            DateTimeOffset dt;
            try
            {
                dt =

                    connection.QueryFirst<DateTimeOffset>
                    (
                        (
                            string.Concat
                                ("SELECT max(Time) FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CpuMetrics], " where AgentId = ", AgentId.ToString())
                        )
                  );
            }
            catch
            {
                dt = DateTimeOffset.FromUnixTimeSeconds(0);
            }
            return dt;

        }

        public void Create(CpuMetric item, int AgentId)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            connection.Execute(string.Concat("INSERT INTO ", MetricsType.metricsList[(int)MetricsTypeEnum.CpuMetrics], "(value, time, AgentId) VALUES(@value, @time, @AgentId)"),
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    AgentId = AgentId
                });
        }

        public void Delete(int AgentId)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CpuMetrics], " WHERE AgentId=@AgentId"),
               new
               {
                   AgentId = AgentId
               });
        }

        public IList<CpuMetric> GetByAgentByPeriod(int AgentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            return connection.Query<CpuMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CpuMetrics], " where AgentId = @AgentId and time>=@from and time<=@to")
                    ),

                    new
                    {
                        AgentId = AgentId,
                        from = fromTime.ToUnixTimeSeconds(),
                        to = toTime.ToUnixTimeSeconds()
                    }
                  ).AsList<CpuMetric>();
        }

        public IList<CpuMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            return connection.Query<CpuMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CpuMetrics], " WHERE time>=@from and time<=@to")
                    ),

                    new
                    {
                        from = fromTime.ToUnixTimeSeconds(),
                        to = toTime.ToUnixTimeSeconds()
                    }
                ).AsList<CpuMetric>();
        }
    }
}
