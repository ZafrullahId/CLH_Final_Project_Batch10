using System;
using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface ISalesServices
    {
         Task<RawMaterialsResponseModel> CalculateAllMonthlyRawMaterialAsync(int year);
        Task<SalesResponseModel> CalculateAllMonthlySalesAsync(int year);
        Task<ProfitResponseModel> CalculateThisMonthProfitAsync();
        Task<ProfitResponseModel> CalculateMonthlyProfitAsync(int month, int year);
        Task<ProfitResponseModel> CalculateThisYearProfitAsync();
        Task<ProfitResponseModel> CalculateYearlyProfitAsync(int year);
        Task<BaseResponse> CreateSales(int id);
        Task<SalesResponseModel> GetAllSales();
        Task<SalesResponseModel> GetSalesByCustomerNameAndDateAsync(string name, DateTime dateOrded);
        Task<SalesResponseModel> GetSalesByCustomerNameAsync(string name);
        Task<SalesResponseModel> GetSalesByProductNameForTheMonth(int productId, int month, int year);
        Task<SalesResponseModel> GetSalesForTheMonthOnEachProduct(int month, int year);
        Task<SalesResponseModel> GetSalesForTheYearOnEachProduct(int year);
        Task<SalesResponseModel> GetSalesForThisMonth();
        Task<SalesResponseModel> GetSalesForThisYear();
    }
}