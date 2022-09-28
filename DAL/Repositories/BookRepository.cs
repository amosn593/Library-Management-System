using DAL.Data;
using DOMAIN.IRepositories;
using DOMAIN.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Book>> GetAll()
        {
            var books = from m in _context.Book
                           select m;
            books = books.OrderByDescending(x => x.RegisterDate)
                .Include(s => s.Form).Include(b => b.BookSource);

            try
            {

                return await books.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetBySerialNo(string SerialNo)
        {
            var books = from m in _context.Book
                           select m;
            books = books.Where(c => c.SerialNumber.Contains(SerialNo))
                .OrderByDescending(x => x.RegisterDate)
                .Include(s => s.Form)
                .Include(b => b.BookSource);
            try
            {
                return await books.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<Book> GetById(int id)
        {
            try
            {
                var entity = await _context.Book.Where(d => d.Id == id)
                   .Include(s => s.BookSource)
                   .Include(f => f.Form).FirstOrDefaultAsync();


                return entity;

            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task<Book> CheckIfIssued(String BookSerialNo)
        {
            try
            {
                var entity = await _context.Book.Where(d => d.SerialNumber.Contains(BookSerialNo))
                    .Where(i => i.Issued=="NO").FirstOrDefaultAsync();


                return entity;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Book> GetSingleBookBySerialNo(string BookSerialNo)
        {
            
            
            try
            {
                var book = await _context.Book.Where(c => c.SerialNumber.Contains(BookSerialNo))
                .FirstOrDefaultAsync();

                return book;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
