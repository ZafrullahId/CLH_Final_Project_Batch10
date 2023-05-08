using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IRwavMaterialServuce
    {
        Task<RawMaterialResponseModel> GetRawAsync(int id);
        Task<BaseResponse> ApproveRawMaterialAsync(int id);
        Task<RawMaterialsResponseModel> GetAllRawMaterials();
        Task<BaseResponse> DeleteRawMaterialRequestAsync(int id);
        Task<RawMaterialsResponseModel> GetAllPendingRawMaterial();
        Task<RawMaterialsResponseModel> GetAllApprovedRawMaterialAsync();
        Task<RawMaterialResponseModel> CalculateRawMaterialCostForThYear();
        Task<RawMaterialResponseModel> CalculateRawMaterialCostForTheMonth();
        Task<RawMaterialsResponseModel> GetAllAprovedRawMateralsForTheYear(int year);
        Task<BaseResponse> CreateRawMaterial(CreateRawMaterialRequestModel model,int id);
        Task<BaseResponse> RejectRawMaterialAsync(int id,RejectRequestRequestModel model);
        Task<RawMaterialsResponseModel> GetAllRejectedRawMaterialForTheYearAsync(int year);
        Task<RawMaterialsResponseModel> GetAllRejectedRawMaterialForTheMonthAsync(int month);
        Task<BaseResponse> UpdateRawMaterialRequestAsync(int id, UpdateRawMaterialRequestModel model);
        Task<RawMaterialsResponseModel> GetAllAprovedRawMaterialsForTheMonthAsync(int month, int year);
    }
}