using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class ChatRepository : BaseRepository<Chat>, IChatRepository
    {
        public ChatRepository(DansnomApplicationContext context)
        {
            _Context = context;
        }
        public async Task<List<Chat>> GetAllChatFromASender(int recieverId, int senderId)
        {
            return await _Context.Chats
            .Include(x => x.Sender)
            .ThenInclude(x => x.User)
            .Where(x => x.SenderId == senderId && x.ReceiverId == recieverId || x.SenderId == recieverId && x.ReceiverId == senderId).OrderBy(x => x.CreatedOn)
            .ToListAsync();
        }
        public async Task<List<Chat>> GetAllUnSeenChatAsync(int recieverId)
        {
            return await _Context.Chats
            .Include(x => x.Sender)
            .Where(x => x.ReceiverId == recieverId && x.Seen == false).ToListAsync();
        }
        public async Task<List<Chat>> GetAllUnSeenChatAsync(int senderId,int recieverId)
        {
            return await _Context.Chats
            .Where(x => x.SenderId == senderId && x.ReceiverId == recieverId && x.Seen == false)
            .ToListAsync();
        }

    }
}