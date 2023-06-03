using System.Threading.Tasks;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;

namespace Interface.Repositories
{
    public interface IPaymentRepository : IBaseRepository<PaymentReference>
    {
        Task<PaymentReference> GetAsync(string transactionReference);
    }
}