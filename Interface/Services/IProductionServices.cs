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
        Task<BaseResponse> RejectProduction(int id);
        Task<BaseResponse> CreateProductionAsync(CreateProductionRequestModel model, List<int> ids);
        Task<ProductionsResponseModel> GetAllPendingProductionsAsync();
        Task<ProductionsResponseModel> GetAllAprovedProductionsByMonthAsync(int month);
        Task<ProductionsResponseModel> GetAllAprovedProductionsByYearAsync(int year);
        Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductByMonthAsync(int year);
        Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductByYearAsync(int year);
        Task<ProductionsResponseModel> GetAllRejectedProductionsByMonthAsync(int month);
        Task<ProductionsResponseModel> GetAllRejectedProductionsByYearAsync(int year);
        Task<ProductionsResponseModel> GetProductionsByDateAsync(string date);
        Task<ProductionsResponseModel> GetProductionsByProductIdAsync(int id);
        Task<BaseResponse> UpdateProductionAsync(int id, UpdateProductionRequestModel model);
    }
}