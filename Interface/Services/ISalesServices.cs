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
        Task<ProfitResponseModel> CalculateMonthlyProfitAsync(int month, int year);
        Task<ProfitResponseModel> CalculateNetProfitAsync(int year, int month, decimal extraExpenses);
        Task<ProfitResponseModel> CalculateThisMonthProfitAsync();
        Task<ProfitResponseModel> CalculateThisYearProfitAsync();
        Task<ProfitResponseModel> CalculateYearlyProfitAsync(int year);
        Task<BaseResponse> CreateSales(int id);
        Task<SalesResponseModel> GetAllSales();
        Task<SalesResponseModel> GetSalesByCustomerNameAsync(string name);
        Task<SalesResponseModel> GetSalesByProductNameForTheMonth(int productId, int month, int year);
        Task<ProductsSaleResponseModel> GetSalesByProductNameForTheYear(int productId, int year);
        Task<OrdersResponseModel> GetSalesForTheMonthOnEachProduct(int month, int year);
        Task<OrdersResponseModel> GetSalesForTheYearOnEachProduct(int year);
        Task<SalesResponseModel> GetSalesForThisMonth();
        Task<SalesResponseModel> GetSalesForThisYear();
    }
}