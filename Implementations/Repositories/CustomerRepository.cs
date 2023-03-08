using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class CustomerRepository : BaseRepository<Customer> , ICustomerRepository
    {
        public CustomerRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<Customer> GetCustomerAsync(string email,string password)
        {
            return await _Context.Customers
            .Where(x => x.User.Email == email && x.User.Password == password && x.IsDeleted == false)
            .Include(x => x.User).SingleOrDefaultAsync();
        }
        public async Task<Customer> GetCustomerByUserIdAsync(int id)
        {
            return await _Context.Customers
            .Where(x => x.User.Id == id && x.User.IsDeleted == false)
            .Include(x => x.User).SingleOrDefaultAsync();
        }
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _Context.Customers
            .Where(x => x.Id == id && x.IsDeleted == false)
            .Include(x => x.User).SingleOrDefaultAsync();
        }
        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _Context.Customers
            .Include(c => c.User)
            .SingleOrDefaultAsync(x => x.User.Email == email && x.IsDeleted == false);
        }
    } 
}