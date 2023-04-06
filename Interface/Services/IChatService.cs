using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IChatService
    {
        Task<BaseResponse> CreateChat(CreateChatRequestModel model, int id, int recieverId);
        Task<ChatsResponseModel> GetChatFromASenderAsync(int senderId, int recieverId);
        Task<BaseResponse> MarkAllChatsAsReadAsync(int senderId, int recieverId);
        Task<ChatsResponseModel> GetAllUnSeenChatAsync(int recieverId);
    }
}