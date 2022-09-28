using DOMAIN.Models;


namespace DOMAIN.IRepositories
{
    public interface IBorrowingRepository : IGenericRepository<Borrowing>
    {
        Task<IEnumerable<Borrowing>> GetByBookSerialNo(string SerialNo);

        Task<IEnumerable<Borrowing>> GetOverDueIssuance();

        Task<Borrowing> GetOverDueIssuanceById(int id);

        Task<IEnumerable<Borrowing>> GetIssuanceHistory();

        Task<Borrowing> GetIssuanceHistoryById(int id);

        Task<IEnumerable<Borrowing>> GetIssuanceHistoryByBookSerialNo(string SerialNo);

        
        Task<IEnumerable<Borrowing>> GetOverDueIssuanceByBookSerialNo(string SerialNo);
    }

    
}
