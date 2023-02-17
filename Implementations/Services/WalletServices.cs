using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;

namespace Dansnom.Implementations.Services
{

    public class WalletServices : IWalletServices
    {
        private readonly IWalletRepository _walletRepository;
        public WalletServices(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public async Task<BaseResponse> CreateWallet(CreateWalletRequestModel model)
        {
            var act = await _walletRepository.GetWallet();
            if (act != null)
            {
                return new BaseResponse
                {
                    Message = "Wallet Already Exist",
                    Success = false
                };
            }
            var wallet = new Wallet
            {
                Total = model.Total
            };
            await _walletRepository.CreateAsync(wallet);
            return new BaseResponse
            {
                Message = "Wallet Successfully Created",
                Success = true
            };
        }
        public async Task<WalletResponseModel> GetWalletById(int id)
        {
            var wallet = await _walletRepository.GetAsync(id);
            if (wallet == null)
            {
                return new WalletResponseModel
                {
                    Message = "Wallet not yet created",
                    Success = false
                };
            }
            return new WalletResponseModel
            {
                Message = $"Wallat amount is {wallet.Total}",
                Success = true,
                Data = new WalletDto
                {
                    Total = wallet.Total
                }
            };
        }
        public async Task<BaseResponse> FundWallet(decimal amount)
        {
            var wallet = await _walletRepository.GetWallet();
            if(wallet == null)
            {
                return new BaseResponse
                {
                    Message = "Wallet not found",
                    Success = false
                };
            }
            wallet.Total += amount;
            await _walletRepository.UpdateAsync(wallet);
            return new BaseResponse
            {
                Message = "Wallet Successfully funded",
                Success = true
            };
        }
    }
}