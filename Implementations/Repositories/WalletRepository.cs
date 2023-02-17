using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class WalletRepository : BaseRepository<Wallet> , IWalletRepository
    {
        public WalletRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<Wallet> GetWallet()
        {
            return await _Context.Wallets.SingleOrDefaultAsync();
        }
    } 
}