using System.Collections.Generic;
using System.Threading.Tasks;

namespace DOMAIN.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        
        void Add(T entity);
        Task Delete(int id);  // make it a task
        void Update(T entity);  // make it a task
    }
}
