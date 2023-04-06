using System.Threading.Tasks;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IProductionRawMaterialService
    {
        Task<ProductionsResponseModel> GetProductionByRwamaterialIdAsync(int id);
    }
}