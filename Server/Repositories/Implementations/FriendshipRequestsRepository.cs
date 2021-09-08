using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using messanger.Server.Data;
using messanger.Server.Models;
using messanger.Server.Repositories.Interfaces;
using messanger.Shared.DTOs.Responses;
using messanger.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace messanger.Server.Repositories.Implementations
{
    public class FriendshipRequestsRepository : IFriendshipRequestsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFriendshipsRepository _friendshipsRepository;

        public FriendshipRequestsRepository(
            ApplicationDbContext context,
            IFriendshipsRepository friendshipsRepository)
        {
            _context = context;
            _friendshipsRepository = friendshipsRepository;
        }

        public async Task<IEnumerable<FriendshipRequestResponseDto>> GetSentFriendshipRequestsAsync(string idUser,
            int skip)
        {
            return await SelectFriendshipRequestsWhereAsync(
                skip,
                fr => fr.IdSender == idUser,
                fr => new FriendshipRequestResponseDto
                {
                    User = new UserResponseDto
                    {
                        IdUser = fr.IdReceiverNavigation.Id,
                        FirstName = fr.IdReceiverNavigation.FirstName,
                        LastName = fr.IdReceiverNavigation.LastName
                    },
                    CreatedAt = fr.CreatedAt
                });
        }

        public async Task<IEnumerable<FriendshipRequestResponseDto>> GetReceivedFriendshipRequestsAsync(string idUser,
            int skip)
        {
            return await SelectFriendshipRequestsWhereAsync(
                skip,
                fr => fr.IdReceiver == idUser,
                fr => new FriendshipRequestResponseDto
                {
                    User = new UserResponseDto
                    {
                        IdUser = fr.IdSenderNavigation.Id,
                        FirstName = fr.IdSenderNavigation.FirstName,
                        LastName = fr.IdSenderNavigation.LastName
                    },
                    CreatedAt = fr.CreatedAt
                });
        }

        public async Task<CreateFriendshipRequestStatus> CreateFriendshipRequestAsync(string idSender,
            string idReceiver)
        {
            if (await _friendshipsRepository.AreUsersInFriendshipAsync(idSender, idReceiver))
                return CreateFriendshipRequestStatus.ALREADY_FRIENDS;

            if (await FriendshipRequestExistsAsync(idSender, idReceiver))
                return CreateFriendshipRequestStatus.ALREADY_EXISTS;

            if (await FriendshipRequestExistsAsync(idReceiver, idSender))
            {
                var isAccepted = await AcceptFriendshipRequestAsync(idReceiver, idSender);
                if (isAccepted)
                    return CreateFriendshipRequestStatus.AUTO_ACCEPTED;

                return CreateFriendshipRequestStatus.CREATE_EXCEPTION;
            }

            await _context.FriendshipRequests.AddAsync(
                new FriendshipRequest
                {
                    IdSender = idSender,
                    IdReceiver = idReceiver,
                    CreatedAt = DateTime.Now
                });

            if (await _context.SaveChangesAsync() > 0)
                return CreateFriendshipRequestStatus.CREATED;

            return CreateFriendshipRequestStatus.CREATE_EXCEPTION;
        }

        public async Task<bool> DeleteFriendshipRequestAsync(string idSender, string idReceiver)
        {
            var fr = await GetFriendshipRequestAsync(idSender, idReceiver);
            if (fr is null)
                return false;

            _context.FriendshipRequests.Remove(fr);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AcceptFriendshipRequestAsync(string idSender, string idReceiver)
        {
            var fr = await GetFriendshipRequestAsync(idSender, idReceiver);
            if (fr is null)
                return false;

            _context.FriendshipRequests.Remove(fr);

            await _context.Friendships.AddAsync(
                new Friendship
                {
                    IdUser1 = idSender,
                    IdUser2 = idReceiver,
                    CreatedAt = DateTime.Now
                });

            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<bool> FriendshipRequestExistsAsync(string idSender, string idReceiver)
        {
            return await _context.FriendshipRequests
                .AnyAsync(fr => fr.IdSender == idSender && fr.IdReceiver == idReceiver);
        }

        private async Task<FriendshipRequest> GetFriendshipRequestAsync(string idSender, string idReceiver)
        {
            return await _context.FriendshipRequests
                .SingleOrDefaultAsync(fr => fr.IdSender == idSender && fr.IdReceiver == idReceiver);
        }

        private async Task<IEnumerable<FriendshipRequestResponseDto>> SelectFriendshipRequestsWhereAsync
        (int skip, Expression<Func<FriendshipRequest, bool>> whereExpression,
            Expression<Func<FriendshipRequest, FriendshipRequestResponseDto>> selectExpression)
        {
            const int take = 10;

            return await _context.FriendshipRequests
                .Where(whereExpression)
                .Include(fr => fr.IdReceiverNavigation)
                .Select(selectExpression)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}