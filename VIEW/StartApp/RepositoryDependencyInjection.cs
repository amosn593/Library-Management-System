using DAL.Data;
using DAL.Repositories;
using DOMAIN.IConfiguration;
using DOMAIN.IRepositories;
using REPORTS.Helpers;

namespace USERVIEW.StartApp
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection RepositoryService(this IServiceCollection Services)
        {
            //Adding AutoMapper DI
            Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            Services.AddScoped<IBookRepository, BookRepository>();
            Services.AddScoped<IBorrowingRepository, BorrowingRepository>();
            Services.AddScoped<IStudentRepository, StudentRepository>();
            Services.AddScoped<IFormRepository, FormRepository>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            return Services;
        }
    }
}
