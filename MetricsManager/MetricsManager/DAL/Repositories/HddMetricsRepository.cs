using MetricsManager.Responses;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Core;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.DAL.Repositories
{
    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке
    public class HddMetricsRepository : IHddMetricsRepository
    {
        public HddMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffSetHandler());
        }

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
                                ("SELECT max(Time) FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.HddMetrics], " where AgentId = ", AgentId.ToString())
                        )
                  );
            }
            catch
            {
                dt = DateTimeOffset.FromUnixTimeSeconds(0);
            }
            return dt;
        }

        public void Create(HddMetric item, int AgentId)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            connection.Execute(string.Concat("INSERT INTO ", MetricsType.metricsList[(int)MetricsTypeEnum.HddMetrics], "(value, time) VALUES(@value, @time)"),
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
        }

        public void Delete(int AgentId)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.HddMetrics], " WHERE AgentId=@AgentId"),
               new
               {
                   AgentId = AgentId
               });
        }

        public IList<HddMetric> GetByAgentByPeriod(int AgentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            return connection.Query<HddMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.HddMetrics], " where AgentId = @AgentId and time>=@from and time<=@to")
                    ),

                    new
                    {
                        AgentId = AgentId,
                        from = fromTime.ToUnixTimeSeconds(),
                        to = toTime.ToUnixTimeSeconds()
                    }
                  ).AsList<HddMetric>();
        }

        public IList<HddMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            return connection.Query<HddMetric>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.HddMetrics], " WHERE time>=@from and time<=@to")
                    ),

                    new
                    {
                        from = fromTime.ToUnixTimeSeconds(),
                        to = toTime.ToUnixTimeSeconds()
                    }
                ).AsList<HddMetric>();
        }
    }
}
