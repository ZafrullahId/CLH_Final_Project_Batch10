using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface ICategoryService
    {
        Task<BaseResponse> CreateCategory(CreateCategoryRequestModel model);
        Task<CategoriesResponseModel> GetAllAsync();
        Task<CategoryResponseModel> GetByIdAsync(int id);
    }
}