using System.Threading.Tasks;
using messanger.Client.Models;
using messanger.Client.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;

namespace messanger.Client.Shared.SideBar
{
    public partial class SearchResults
    {
        [Parameter] public SearchResultsWrapper WrappedResults { get; set; }

        [Parameter] public EventCallback StopSearchingCallback { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IConversationsRepository ConversationsRepository { get; set; }

        private async Task CloseResults()
        {
            await StopSearchingCallback.InvokeAsync();
        }
    }
}