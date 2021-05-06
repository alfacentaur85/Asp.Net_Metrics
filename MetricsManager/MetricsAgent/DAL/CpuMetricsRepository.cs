using MetricsAgent.Responses;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.DAL
{
    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке
    public interface ICpuMetricsRepository : IRepository<Metric>
    {

    }

    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        // инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(Metric item)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);

            connection.Execute(string.Concat("INSERT INTO ", MetricsType.metricsList[(int)MetricsTypeEnum.CPU_METRICS], "(value, time) VALUES(@value, @time)"),
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Execute(string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CPU_METRICS], " WHERE id=@id"),
               new
               {
                   id = id
               });
        }

        public void Update(Metric item)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Execute(string.Concat("UPDATE ", MetricsType.metricsList[(int)MetricsTypeEnum.CPU_METRICS], " SET value = @value, time = @time WHERE id=@id"),
               new
               {
                   id = item.Id,
                   value = item.Value,
                   time = item.Time.ToUnixTimeSeconds()
               });
        }

        public IList<Metric> GetAll()
        {
            using var connection = new SQLiteConnection(Startup.connectionString);

            return connection.Query<Metric> 
                (
                    (
                      string.Concat
                             ("SELECT id, value, datetime(time, 'unixepoch', 'localtime') time FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CPU_METRICS])
                    )
                  ).AsList<Metric>();
        }

        public Metric GetById(int id)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = string.Concat("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CPU_METRICS]," WHERE id=@id");
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new Metric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = DateTimeOffset.Parse(reader.GetString(2))
                    };
                }
                else
                {
                    // не нашлось запись по идентификатору, не делаем ничего
                    return null;
                }
            }
        }

        public IList<Metric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = string.Concat("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.CPU_METRICS], " WHERE time>=@from and time<=@to");
            var returnList = new List<Metric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new Metric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        // налету преобразуем прочитанные секунды в метку времени
                        Time = DateTimeOffset.Parse(reader.GetString(2))
                    });
                }
            }
            return returnList;
        }
    }
}
