using System;
using System.Linq;
using System.Threading.Tasks;
using messanger.Server.Data;
using messanger.Server.Models;
using messanger.Server.Repositories.Interfaces;
using messanger.Shared.DTOs;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace messanger.Server.Repositories.Implementations
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ApplicationDbContext _context;

        public MessagesRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageResponseDto> AddNewMessageAsync(string idSender, NewMessageRequestDto newMessage)
        {
            var conversation = await _context.Conversations
                .SingleOrDefaultAsync
                (c => c.IdConversation == newMessage.IdConversation &&
                      c.ConversationMembers.Any(cm => cm.IdUserNavigation.Id == idSender));

            if (conversation is null)
                return null;

            var message = new Message
            {
                Content = newMessage.Content,
                CreatedAt = DateTime.Now,
                IdConversationNavigation = conversation,
                IdSender = idSender
            };

            conversation.Messages.Add(message);

            if (await _context.SaveChangesAsync() <= 0)
                return null;

            return new MessageResponseDto
            {
                IdMessage = message.IdMessage,
                Content = message.Content,
                CreatedAt = message.CreatedAt,
                Sender = await _context.Users
                    .Where(u => u.Id == idSender)
                    .Select(u => new UserResponseDto
                    {
                        IdUser = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }).SingleOrDefaultAsync()
            };
        }
    }
}
