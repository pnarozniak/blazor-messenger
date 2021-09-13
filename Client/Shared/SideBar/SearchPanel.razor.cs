using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using messanger.Client.Models;
using messanger.Client.Repositories.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace messanger.Client.Shared.SideBar
{
    public partial class SearchPanel
    {
        private string _searchInputValue;

        private bool IsSearchInputValueValid => !string.IsNullOrWhiteSpace(_searchInputValue) &&
                                                Regex.IsMatch(_searchInputValue, "^[a-zA-Z\\d\\s]+$");

        [Parameter] public bool IsSearching { get; set; }

        [Parameter] public EventCallback<bool> IsSearchingChanged { get; set; }

        [Parameter] public SearchResultsWrapper SearchResultsWrapper { get; set; }

        [Parameter] public EventCallback<SearchResultsWrapper> SearchResultsWrapperChanged { get; set; }

        [Inject] public IConversationsRepository ConversationsRepository { get; set; }

        [Inject] public IFriendshipsRepository FriendshipsRepository { get; set; }

        [Inject] public IUsersRepository UsersRepository { get; set; }

        private async Task ToggleSearching(bool isSearching)
        {
            if (isSearching == false) await ClearSearch();

            IsSearching = isSearching;
            await IsSearchingChanged.InvokeAsync(isSearching);
        }

        public async Task ClearSearch()
        {
            _searchInputValue = string.Empty;
            SearchResultsWrapper = new SearchResultsWrapper();
            await NotifySearchResultsChanged();
        }

        private async Task OnKeyUp(KeyboardEventArgs args)
        {
            if (!IsSearching)
                await ToggleSearching(true);

            if (string.IsNullOrWhiteSpace(_searchInputValue))
            {
                SearchResultsWrapper = new SearchResultsWrapper();
                await NotifySearchResultsChanged();
                return;
            }

            if (args is not null
                && args.Key != "Backspace"
                && !Regex.IsMatch(args.Key, "^[a-zA-Z\\d]$"))
                return;

            await FetchSearchResults();
        }

        private async Task FetchSearchResults()
        {
            SearchResultsWrapper.Conversations = await FetchConversations();
            await NotifySearchResultsChanged();

            SearchResultsWrapper.Friends = await FetchFriends();
            await NotifySearchResultsChanged();

            SearchResultsWrapper.MorePeople = await FetchMorePeople();
            await NotifySearchResultsChanged();
        }

        private async Task NotifySearchResultsChanged()
        {
            await SearchResultsWrapperChanged.InvokeAsync(SearchResultsWrapper);
        }

        private async Task<IEnumerable<ConversationResponseDto>> FetchConversations()
        {
            return await ConversationsRepository
                .GetConversationsAsync(new GetConversationsRequestDto { Filter = _searchInputValue });
        }

        private async Task<IEnumerable<UserResponseDto>> FetchFriends()
        {
            return await FriendshipsRepository
                .GetFriendsAsync(new GetFriendsRequestDto
                { Filter = _searchInputValue, OnlyWithoutPrivateConversation = true });
        }

        private async Task<IEnumerable<UserResponseDto>> FetchMorePeople()
        {
            return await UsersRepository
                .GetUsersAsync(new GetUsersRequestDto { Filter = _searchInputValue });
        }
    }
}