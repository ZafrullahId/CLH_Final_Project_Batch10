using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IAddressService
    {
        Task<AddressResponseModel> GetAddressAsync(int id);
    }
}