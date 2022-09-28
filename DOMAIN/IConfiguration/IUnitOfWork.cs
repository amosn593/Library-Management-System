using DOMAIN.IRepositories;


namespace DOMAIN.IConfiguration
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Book { get; }
        IBorrowingRepository Borrowing { get; }
        IStudentRepository Student { get; }
        IFormRepository Form { get; }



        Task Complete();
    }
}
