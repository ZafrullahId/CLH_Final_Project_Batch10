using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IProductServices
    {
        Task<BaseResponse> CreateProduct(CreateProductRequestModel model);
        Task<BaseResponse> DeleteProductAsync(int id);
        Task<ProductsResponseModel> GetAllProducts();
        Task<ProductResponseModel> GetProductById(int id);
        Task<BaseResponse> UpdateProduct(UpdateProductRequestModel model,int id);
        Task<ProductsResponseModel> GetAvailableProductsAsync();
        Task<BaseResponse> UpdateProductsAvailability();
        Task<ProductsResponseModel> GetProductsByCategoryId(int id);
    }
}