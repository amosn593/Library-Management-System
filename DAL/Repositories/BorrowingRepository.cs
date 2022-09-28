using DAL.Data;
using DOMAIN.IRepositories;
using DOMAIN.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BorrowingRepository : GenericRepository<Borrowing>, IBorrowingRepository
    {
        public BorrowingRepository(LibraryDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Borrowing>> GetAll()
        {
            var borrowings = from m in _context.Borrowing
                             select m;
                       
            
            try
            {
                borrowings = borrowings.Where(i => i.Issued == "YES")
                    .OrderByDescending(d => d.RegisterDate)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(f => f.Form)
                    .Include(s => s.CurrentStudent);
                 
                 return await borrowings.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Borrowing>> GetByBookSerialNo(string SerialNo)
        {
            var borrowings = from m in _context.Borrowing
                             select m;


            try
            {
                borrowings = borrowings.Where(i => i.Issued == "YES")
                    .OrderByDescending(d => d.RegisterDate)
                    .Include(b => b.CurrentBook)
                    .Where(sn => sn.CurrentBook.SerialNumber == SerialNo)
                    .Include(s => s.CurrentStudent).ThenInclude(f => f.Form);

                return await borrowings.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<Borrowing>  GetById(int id)
        {
            
            try
            {
                var entity = await _context.Borrowing.Where(i => i.Id == id)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.Form)
                    .Include(s => s.CurrentStudent)
                    .FirstOrDefaultAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

       
        public async Task<IEnumerable<Borrowing>> GetIssuanceHistory()
        {
            var borrowings = from m in _context.Borrowing
                             select m;
            try
            {
                borrowings = borrowings.Where(i => i.Issued == "NO")
                    .OrderByDescending(d => d.RegisterDate)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(f => f.Form)
                    .Include(s => s.CurrentStudent);
                    

                return await borrowings.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Borrowing>> GetIssuanceHistoryByBookSerialNo(string SerialNo)
        {
            var borrowings = from m in _context.Borrowing
                             select m;
            try
            {
                borrowings = borrowings.Where(i => i.Issued == "NO")
                    .Where(b => b.CurrentBook.SerialNumber.Contains(SerialNo))
                    .OrderByDescending(d => d.RegisterDate)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(f => f.Form)
                    .Include(s => s.CurrentStudent);
                    

                return await borrowings.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        public async Task<IEnumerable<Borrowing>> GetOverDueIssuance()
        {
            var borrowings = from m in _context.Borrowing
                             select m;
            try
            {
                borrowings = borrowings
                    .Where(b => b.Issued == "Yes")
                    .Where(b => b.ReturnDate <= DateTime.Now)
                    .OrderByDescending(d => d.RegisterDate)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(f => f.Form)
                    .Include(b => b.CurrentStudent);
                    

                return await borrowings.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Borrowing> GetOverDueIssuanceById(int id)
        {
            
            try
            {
                var entity = await _context.Borrowing
                    .Where(b => b.Issued == "Yes")
                    .Where(i => i.Id == id)
                    .Where(b => b.ReturnDate <= DateTime.Now)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(f => f.Form)
                    .Include(b => b.CurrentStudent)
                    .FirstOrDefaultAsync();


                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Borrowing>> GetOverDueIssuanceByBookSerialNo(string SerialNo)
        {
            var borrowings = from m in _context.Borrowing
                             select m;
            try
            {
                borrowings = borrowings
                    .OrderByDescending(d => d.RegisterDate)
                    .Where(b => b.Issued == "Yes")
                    .Where(b => b.ReturnDate <= DateTime.Now)
                    .OrderBy(d => d.RegisterDate)
                    .Include(b => b.CurrentBook)
                    .Where(o => o.CurrentBook.SerialNumber.Contains(SerialNo))
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(f => f.Form)
                    .Include(b => b.CurrentStudent);
                   

                return await borrowings.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Borrowing> GetIssuanceHistoryById(int id)
        {

            try
            {
                var entity = await _context.Borrowing
                    .Where(b => b.Issued == "NO")
                    .Where(i => i.Id == id)
                    .Include(b => b.CurrentBook).ThenInclude(c => c.BookSource)
                    .Include(b => b.CurrentBook).ThenInclude(f => f.Form)
                    .Include(b => b.CurrentStudent)
                    .FirstOrDefaultAsync();


                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

        
}
