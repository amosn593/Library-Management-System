using DAL.Data;
using DOMAIN.IRepositories;
using DOMAIN.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FormRepository : GenericRepository<Form>, IFormRepository
    {
        public FormRepository(LibraryDbContext context) : base(context)
        {
        }


        public override async Task<IEnumerable<Form>> GetAll()
        {
            var classes = from m in _context.Form
                           select m;
            classes = classes.OrderByDescending(x => x.Id);
                

            try
            {

                return await classes.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
