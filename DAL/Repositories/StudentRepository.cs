using DAL.Data;
using DOMAIN.IRepositories;
using DOMAIN.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(LibraryDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<Student>> GetAll()
        {
            var students = from m in _context.Student
                             select m;
            students = students.OrderByDescending(x => x.Id)
                .Include(s => s.Form);

            try
            {
                
                return await students.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Student>> GetByAdminNo(string AdminNo)
        {
            var students = from m in _context.Student
                           select m;
            students = students.Where(c => c.AdminNumber.Contains(AdminNo))
                .Include(s => s.Form);
            try
            {
                return await students.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<Student> GetById(int id)
        {
            try
            {
                var entity = await _context.Student.Where(d => d.Id == id)
                   .Include(f => f.Form).FirstOrDefaultAsync();


                return entity;

            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task<Student> CheckIfActive(String AdminNo)
        {
            try
            {
                var entity = await _context.Student.Where(d => d.AdminNumber.Contains(AdminNo))
                    .Where(i => i.Active == "YES")
                    .FirstOrDefaultAsync();


                return entity;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfIssuedBook(int id)
        {
            try
            {
                var entity = await _context.Student.Where(d => d.Id==id)
                    .Where(i => i.Borrowings.Where(b => b.Issued=="YES").Any())
                    .FirstOrDefaultAsync();

                if(entity == null)
                {
                    return false;
                }


                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
