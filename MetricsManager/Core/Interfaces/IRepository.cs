using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        IList<T> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        void Create(T item);
        T GetById(int id);
        void Delete(int id);


    }
}
