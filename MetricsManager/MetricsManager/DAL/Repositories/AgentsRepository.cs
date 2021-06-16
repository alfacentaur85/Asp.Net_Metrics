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

    public class AgentsRepository : IAgentsRepositorySingle
    {
        public AgentsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffSetHandler());
        }
       
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public void Create(Agents item)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            connection.Execute(string.Concat("INSERT INTO ", MetricsType.metricsList[(int)MetricsTypeEnum.Agents], "(AgentURL) VALUES(@AgentURL)"),
                new
                {
                    AgentURL = item.AgentURL
                }) ;
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.Agents], " WHERE AgentId=@AgentId"),
               new
               {
                   AgentId = id
               });
        }
        public void Delete()
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.Agents]));
        }
        public void Update(Agents item)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            connection.Execute(string.Concat("UPDATE ", MetricsType.metricsList[(int)MetricsTypeEnum.CpuMetrics], " SET AgentURL = @AgentURL where AgentId = @AgentId"),
               new
               {
                   AgentId = item.AgentId,
                   AgentURL = item.AgentURL,
               });
        }

        public IList<Agents> GetAll()
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);

            return  connection.Query<Agents> 
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.Agents])
                    )
                  ).AsList<Agents>();
        }
        public Agents GetById(int id)
        {
            using var connection = new SQLiteConnection(Startup.ConnectionString);
            return connection.QuerySingle<Agents>
                (
                    (
                      string.Concat
                             ("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.Agents])
                    ),

                    new
                    {
                        AgentId = id
                    }
                );
        }
        public IList<Agents> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            return null;
        }
    }
}
