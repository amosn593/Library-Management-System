
using DOMAIN.Models;

namespace DOMAIN.IRepositories
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IEnumerable<Book>> GetBySerialNo(string BookSerialNo);

        Task<Book> GetSingleBookBySerialNo(string BookSerialNo);

        Task<Book> CheckIfIssued(String BookSerialNo);
    }
}
