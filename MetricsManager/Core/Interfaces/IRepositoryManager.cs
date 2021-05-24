using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepositoryManager<T> where T : class
    {
        IList<T> GetByAgentByPeriod(int AgentID, DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<T> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        void Create(T item, int AgentId);
        void Delete(int id);

        DateTimeOffset GetMaxDate(int AgentId);


    }
}
