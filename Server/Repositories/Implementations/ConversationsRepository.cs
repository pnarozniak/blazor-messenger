using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using messanger.Server.Data;
using messanger.Server.Helpers;
using messanger.Server.Repositories.Interfaces;
using messanger.Shared.DTOs;
using messanger.Shared.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace messanger.Server.Repositories.Implementations
{
    public class ConversationsRepository : IConversationsRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConversationResponseDto>> GetUserRecentConversationsAsync
            (string idUser, int skip)
        {
            const int takeConversations = 10;
            const int takeConversationMembers = 5;

            return await _context.Conversations
                .Where(c => c.ConversationMembers.Any(cm => cm.IdUser == idUser))
                .OrderByDescending(c => c.Messages.OrderBy(m => m.CreatedAt).FirstOrDefault().CreatedAt)
                .Select(c => new ConversationResponseDto
                {
                    IdConversation = c.IdConversation,
                    Name = c.Name,
                    Members = c.ConversationMembers
                        .Where(cm => cm.IdUserNavigation.Id != idUser)
                        .Select(cm => new UserResponseDto
                        {
                            FirstName = cm.IdUserNavigation.FirstName,
                            LastName = cm.IdUserNavigation.LastName
                        })
                        .Take(takeConversationMembers),
                    LastMessage = c.Messages
                        .OrderByDescending(m => m.CreatedAt).Select(m => new MessageResponseDto
                        {
                            IdMessage = m.IdMessage,
                            Content = m.Content,
                            CreatedAt = m.CreatedAt
                        }).FirstOrDefault()
                })
                .Skip(skip)
                .Take(takeConversations)
                .ToListAsync();
        }

        public async Task<IEnumerable<ConversationResponseDto>> GetUserConversationsMatchingFilterAsync
            (string idUser, string filter)
        {
            const int takeConversations = 10;
            const int takeConversationMembers = 5;

            var firstName = FilterHelper.GetFirstPart(filter);
            var lastName = FilterHelper.GetLastPart(filter);

            return await _context.Conversations
                .Where(c => c.ConversationMembers.Any(cm => cm.IdUser == idUser))
                .Where(c => c.Name != null && c.Name.StartsWith(filter) || c.ConversationMembers.Any(cm =>
                    cm.IdUserNavigation.FirstName.StartsWith(firstName) &&
                    cm.IdUserNavigation.LastName.StartsWith(lastName)))
                .OrderBy(c => c.Name == null ? 2 : c.Name.StartsWith(filter) ? 0 : 1)
                .Select(c => new ConversationResponseDto
                {
                    IdConversation = c.IdConversation,
                    Name = c.Name,
                    Members = c.ConversationMembers
                        .Where(cm => cm.IdUserNavigation.Id != idUser)
                        .OrderBy(cm => cm.IdUserNavigation.FirstName.StartsWith(firstName) ? 0 : 1)
                        .ThenBy(cm => cm.IdUserNavigation.LastName.StartsWith(lastName) ? 0 : 1)
                        .Select(cm => new UserResponseDto
                        {
                            FirstName = cm.IdUserNavigation.FirstName,
                            LastName = cm.IdUserNavigation.LastName
                        })
                        .Take(takeConversationMembers),
                    LastMessage = c.Messages
                        .OrderByDescending(m => m.CreatedAt).Select(m => new MessageResponseDto
                        {
                            IdMessage = m.IdMessage,
                            Content = m.Content,
                            CreatedAt = m.CreatedAt
                        }).FirstOrDefault()
                })
                .Take(takeConversations)
                .ToListAsync();
        }

        public async Task<IEnumerable<MessageResponseDto>> GetUserConversationMessagesAsync
            (int idConversation, string idUser, int skip)
        {
            return await _context.Messages
                .Where(m => m.IdConversation == idConversation
                            && m.IdConversationNavigation.ConversationMembers
                                .Any(cm => cm.IdUserNavigation.Id == idUser))
                .OrderBy(m => m.CreatedAt)
                .Skip(skip)
                .Select(m => new MessageResponseDto
                {
                    IdMessage = m.IdMessage,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    Sender = new UserResponseDto
                    {
                        IdUser = m.IdSenderNavigation.Id,
                        FirstName = m.IdSenderNavigation.FirstName,
                        LastName = m.IdSenderNavigation.LastName
                    }
                }).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetConversationMembersIdsAsync
            (int idConversation)
        {
            return await _context.ConversationMembers
                .Where(cm => cm.IdConversation == idConversation)
                .Select(cm => cm.IdUserNavigation.Id)
                .ToListAsync();
        }
    }
}