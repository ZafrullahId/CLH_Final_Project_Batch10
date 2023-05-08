using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IProductionServices
    {
        Task<BaseResponse> RejectProduction(int id, RejectRequestRequestModel model);
         Task<BaseResponse> AproveProduction(int id);
        Task<ProductionsResponseModel> GetAllProductionAsync();
        Task<ProductionResponseModel> GetProductionById(int id);
        Task<ProductionsResponseModel> GetAllAprovedProductionsAsync();
        Task<ProductionsResponseModel> GetAllPendingProductionsAsync();
        Task<ProductionsResponseModel> GetAllRejectedProductionsAsync();
        Task<ProductionsResponseModel> GetProductionsByProductIdAsync(int id);
        Task<ProductionsResponseModel> GetProductionsByDateAsync(string date);
        Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductAsync();
        Task<BaseResponse> UpdateProductionAsync(int id, UpdateProductionRequestModel model);
        Task<ProductionsResponseModel> GetAllAprovedProductionsByMonthAsync(int year, int month);
        Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductByMonthAsync(int month);
        Task<BaseResponse> CreateProductionAsync(CreateProductionRequestModel model, List<int> ids, int adminId);
    }
}