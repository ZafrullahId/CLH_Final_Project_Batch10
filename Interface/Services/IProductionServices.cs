using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IProductionServices
    {
         Task<BaseResponse> AproveProduction(int id);
        Task<BaseResponse> CreateProductionAsync(CreateProductionRequestModel model, List<int> ids, int adminId);
        Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductByMonthAsync(int month);
        Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductAsync();
        Task<ProductionsResponseModel> GetAllAprovedProductionsAsync();
        Task<ProductionsResponseModel> GetAllAprovedProductionsByMonthAsync(int year, int month);
        Task<ProductionsResponseModel> GetAllPendingProductionsAsync();
        Task<ProductionsResponseModel> GetAllRejectedProductionsAsync();
        Task<ProductionsResponseModel> GetProductionsByDateAsync(string date);
        Task<ProductionsResponseModel> GetProductionsByProductIdAsync(int id);
        Task<BaseResponse> RejectProduction(int id);
        Task<BaseResponse> UpdateProductionAsync(int id, UpdateProductionRequestModel model);
    }
}