using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class ChatResponseModel : BaseResponse
    {
        public ChatDto Data { get; set; }
    }
    public class ChatsResponseModel : BaseResponse
    {
        public List<ChatDto> Data { get; set; }
    }
}