using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public  interface IWalletRepository : IBaseRepository<Wallet>
    {
        Task<Wallet> GetWallet();
    }
    
}