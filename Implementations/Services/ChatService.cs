using System;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;

namespace Dansnom.Implementations.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IAdminRepository _adminRepository;
        public ChatService(IChatRepository chatRepository, IAdminRepository adminRepository)
        {
            _chatRepository = chatRepository;
            _adminRepository = adminRepository;
        }
        public async Task<BaseResponse> CreateChat(CreateChatRequestModel model, int id, int recieverId)
        {
            var sender = await _adminRepository.GetAdminByUserIdAsync(id);
            var receiver = await _adminRepository.GetAdminByUserIdAsync(recieverId);
            if (sender == null || receiver == null)
            {
                return new BaseResponse
                {
                    Message = "Opps Something Bad went wrong",
                    Success = false
                };
            }
            var chat = new Chat
            {
                Message = model.Message,
                PostedTime = DateTime.Now.ToLongDateString(),
                SenderId = sender.Id,
                ReceiverId = receiver.Id
            };
            await _chatRepository.CreateAsync(chat);
            return new BaseResponse
            {
                Message = " Message successfully sent",
                Success = true
            };
        }
        public async Task<ChatsResponseModel> GetChatFromASenderAsync(int senderId, int recieverId)
        {
            var sender = await _adminRepository.GetAdminByUserIdAsync(senderId);
            var receiver = await _adminRepository.GetAdminByUserIdAsync(recieverId);
            if (sender == null || receiver == null)
            {
                return new ChatsResponseModel
                {
                    Message = "Opps Something Bad went wrong",
                    Success = false
                };
            }
            var chats = await _chatRepository.GetAllChatFromASender(receiver.Id, sender.Id);
            foreach (var chat in chats)
            {
                if ((DateTime.Now - chat.CreatedOn).TotalSeconds < 60)
                {
                    chat.PostedTime = (int)(DateTime.Now - chat.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - chat.CreatedOn).TotalSeconds > 60 && (DateTime.Now - chat.CreatedOn).TotalHours < 1)
                {
                    chat.PostedTime = (int)(DateTime.Now - chat.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - chat.CreatedOn).TotalMinutes > 60 && (DateTime.Now - chat.CreatedOn).TotalDays < 1)
                {
                    chat.PostedTime = (int)(DateTime.Now - chat.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - chat.CreatedOn).TotalHours > 24 && (DateTime.Now - chat.CreatedOn).TotalDays < 30)
                {
                    chat.PostedTime = (int)(DateTime.Now - chat.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - chat.CreatedOn).TotalDays > 30 && (DateTime.Now - chat.CreatedOn).TotalDays <= 365)
                {
                    chat.PostedTime = ((int)(DateTime.Now - chat.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }

            }
            return new ChatsResponseModel
            {
                Message = "Chats restored successfully",
                Success = true,
                Data = chats.Select(x => new ChatDto
                {
                    SenderId = x.Sender.UserId,
                    Message = x.Message,
                    PostedTime = x.CreatedOn.ToShortTimeString(),
                    Seen = x.Seen,
                    profileImage = x.Sender.User.ProfileImage
                }).ToList()
            };
        }
        public async Task<BaseResponse> GetAllUnSeenChatAsync(int recieverId)
        {
            var receiver = await _adminRepository.GetAdminByUserIdAsync(recieverId);
            if (receiver == null)
            {
                return new BaseResponse
                {
                    Message = "Opps Something Bad went wrong",
                    Success = false
                };
            }
            var unseen = _chatRepository.GetAllUnSeenChatAsync(receiver.Id);
           
            return new BaseResponse
            {
                Message = unseen.ToString(),
                Success = true,
            };
        }
        public async Task<BaseResponse> MarkAllChatsAsReadAsync(int senderId, int recieverId)
        {
            var sender = await _adminRepository.GetAdminByUserIdAsync(senderId);
            var receiver = await _adminRepository.GetAdminByUserIdAsync(recieverId);
            if (sender == null || receiver == null)
            {
                return new BaseResponse
                {
                    Message = "Opps Something Bad went wrong",
                    Success = false
                };
            }

            var chats = await _chatRepository.GetAllUnSeenChatAsync(sender.Id, receiver.Id);
            foreach (var chat in chats)
            {
                chat.Seen = true;
                await _chatRepository.UpdateAsync(chat);
            }
            return new BaseResponse
            {
                Message = "Messages marked as seen",
                Success = true
            };
        }
    }
}