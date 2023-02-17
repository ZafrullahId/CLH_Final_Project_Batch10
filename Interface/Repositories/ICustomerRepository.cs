using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer> GetCustomerByUserIdAsync(int id);
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer> GetCustomerByIdAsync(int id);
    }
    
}