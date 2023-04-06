using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IChatRepository : IBaseRepository<Chat>
    {
        Task<List<Chat>> GetAllUnSeenChatAsync(int recieverId);
        Task<List<Chat>> GetAllUnSeenChatAsync(int senderId,int recieverId);
        Task<List<Chat>> GetAllChatFromASender(int recieverId, int senderId);
    }
}