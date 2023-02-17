using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IWalletServices
    {
        Task<BaseResponse> CreateWallet(CreateWalletRequestModel model);
        Task<WalletResponseModel> GetWalletById(int id);
        Task<BaseResponse> FundWallet(decimal amount);
    }
}