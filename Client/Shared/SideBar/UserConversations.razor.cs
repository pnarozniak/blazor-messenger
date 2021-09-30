using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Components;

namespace messanger.Client.Shared.SideBar
{
    public partial class UserConversations : IDisposable
    {
        private Timer _timer;

        [Parameter] public Func<ElementReference> GetListContainerRef { get; set; }

        [Inject] public IAppStateService AppStateService { get; set; }

        [Inject] public IConversationsRepository ConversationsRepository { get; set; }

        protected override void OnInitialized()
        {
            AppStateService.OnChange += StateHasChanged;
            RunRefreshTimer();
        }

        private void RunRefreshTimer()
        {
            _timer = new Timer(60000);
            _timer.Elapsed += (_, _) => StateHasChanged();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public async Task<bool> FetchConversationsAsync()
        {
            var newConversations = (await ConversationsRepository
                    .GetConversationsAsync(
                        new GetConversationsRequestDto { Skip = AppStateService.Conversations.Count }))!
                .ToList();

            if (!newConversations.Any())
            {
                return false;
            }

            AppStateService.AddConversations(newConversations);
            return true;
        }

        public void Dispose()
        {
            AppStateService.OnChange -= StateHasChanged;
            _timer.Dispose();
        }
    }
}