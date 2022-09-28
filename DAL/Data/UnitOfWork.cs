using DOMAIN.IConfiguration;
using DOMAIN.IRepositories;


namespace DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly LibraryDbContext _context;
        public IBookRepository Book { get; }

        public IBorrowingRepository Borrowing { get; }

        public IStudentRepository Student { get; }

        public IFormRepository Form { get; }

        public UnitOfWork(LibraryDbContext context, IBookRepository booksRepository,
            IBorrowingRepository borrowingRepository, IStudentRepository studentRepository,
            IFormRepository formRepository)
        {
            _context = context;

            Book = booksRepository;
            Borrowing = borrowingRepository;
            Student = studentRepository;
            Form = formRepository;

        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
