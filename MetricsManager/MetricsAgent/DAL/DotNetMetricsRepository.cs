﻿using MetricsAgent.Responses;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке
    public interface IDotNetMetricsRepository : IRepository<Metric>
    {

    }

    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        // инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(Metric item)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Open();
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = string.Concat("INSERT INTO ", MetricsType.metricsList[(int)MetricsTypeEnum.DOTNET_METRICS], "(value, time) VALUES(@value, @time)");

            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@value", item.Value);

            // в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
            // через свойство
            cmd.Parameters.AddWithValue("@time", item.Time.Second);
            // подготовка команды к выполнению
            cmd.Prepare();
            // выполнение команды
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на удаление данных
            cmd.CommandText = string.Concat("DELETE FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DOTNET_METRICS], " WHERE id=@id");

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(Metric item)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = string.Concat("UPDATE ", MetricsType.metricsList[(int)MetricsTypeEnum.DOTNET_METRICS], " SET value = @value, time = @time WHERE id=@id;");
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.Second);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<Metric> GetAll()
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = string.Concat("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DOTNET_METRICS]);

            var returnList = new List<Metric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new Metric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        // налету преобразуем прочитанные секунды в метку времени
                        Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
                    });
                }
            }

            return returnList;
        }

        public Metric GetById(int id)
        {
            using var connection = new SQLiteConnection(Startup.connectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = string.Concat("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DOTNET_METRICS], " WHERE id=@id");
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
                        Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(1))
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
            cmd.CommandText = string.Concat("SELECT * FROM ", MetricsType.metricsList[(int)MetricsTypeEnum.DOTNET_METRICS], " WHERE time>=@from and time<=@to");
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
                        Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }
    }
}
