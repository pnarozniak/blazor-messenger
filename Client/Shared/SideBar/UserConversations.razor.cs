using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace messanger.Client.Shared.SideBar
{
    public partial class UserConversations : IAsyncDisposable
    {
        private ConversationsList _conversationsListRef;

        private bool _hasFetchedAllConversations;

        private bool _isFetching;
        private bool _showSkeletonLoading;

        private Timer _timer;

        private IJSObjectReference _userConversationsJsModule;

        [Parameter] public Func<ElementReference> GetListContainerRef { get; set; }

        [Inject] public IJSRuntime JsRuntime { get; set; }

        [Inject] public IAppStateService AppStateService { get; set; }

        [Inject] public IConversationsRepository ConversationsRepository { get; set; }

        public async ValueTask DisposeAsync()
        {
            AppStateService.OnChange -= StateHasChanged;

            _timer.Dispose();

            if (_userConversationsJsModule is not null)
                await _userConversationsJsModule.DisposeAsync();
        }

        protected override void OnInitialized()
        {
            AppStateService.OnChange += StateHasChanged;

            _showSkeletonLoading = !AppStateService.Conversations.Any();

            RunRefreshTimer();
        }

        private void RunRefreshTimer()
        {
            _timer = new Timer(60000);
            _timer.Elapsed += (_, _) => _conversationsListRef.Refresh();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            _userConversationsJsModule = await JsRuntime.InvokeAsync<IJSObjectReference>
                ("import", "./js/UserConversations.js").AsTask();
            await _userConversationsJsModule.InvokeVoidAsync("runListContainerListeners",
                GetListContainerRef(), DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public async Task<bool> FetchConversationsAsync()
        {
            if (_hasFetchedAllConversations || _isFetching)
                return false;

            _isFetching = true;

            var newConversations = (await ConversationsRepository
                    .GetConversationsAsync(
                        new GetConversationsRequestDto { Skip = AppStateService.Conversations.Count }))!
                .ToList();

            if (!newConversations.Any())
            {
                _hasFetchedAllConversations = true;
                _showSkeletonLoading = false;
                StateHasChanged();

                _isFetching = false;
                return false;
            }

            AppStateService.AddConversations(
                newConversations
                    .Where(newConversation =>
                        AppStateService.Conversations.All(c => c.IdConversation != newConversation.IdConversation))
                    .ToList()
            );

            _isFetching = false;
            return true;
        }
    }
}