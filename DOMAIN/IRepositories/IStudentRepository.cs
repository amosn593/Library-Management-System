using DOMAIN.Models;

namespace DOMAIN.IRepositories
{
    public interface IStudentRepository: IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetByAdminNo(string AdminNo);

        Task<Student> CheckIfActive(String AdminNo);

        Task<bool> CheckIfIssuedBook(int id);
    }
}
