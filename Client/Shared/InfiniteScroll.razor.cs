using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace messanger.Client.Shared
{
    public partial class InfiniteScroll<TItem> : IAsyncDisposable
    {
        private bool _hasAll;
        private bool _loading;
        private ElementReference _lastItemRef;
        private IJSObjectReference _module;
        private IJSObjectReference _instance;

        [Parameter] public List<TItem> Items { get; set; }

        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

        [Parameter] public Func<ElementReference> GetScrollContainerRef { get; set; }

        [Parameter] public RenderFragment LoadingTemplate { get; set; }

        [Parameter] public RenderFragment HasAllTemplate { get; set; }

        [Parameter] public Func<Task<bool>> LoadNextItems { get; set; }

        [Parameter] public float ItemSize { get; set; } = 50f;

        [Parameter] public bool ScrollReverse { get; set; } = false;

        [Inject] public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/infinite-scroll.js");
                await RunObserver();
            }
        }

        private async Task RunObserver()
        {
            _instance = await _module.InvokeAsync<IJSObjectReference>("initialize", new
            {
                scrollContainer = GetScrollContainerRef(),
                lastItem = _lastItemRef,
                componentInstance = DotNetObjectReference.Create(this),
                scrollReverse = ScrollReverse
            });
        }

        [JSInvokable]
        public async Task OnScrollToEnd()
        {
            if (_loading)
                return;

            _loading = true;
            StateHasChanged();

            _hasAll = !await LoadNextItems.Invoke();
            _loading = false;
            StateHasChanged();
        }

        public async Task ForceRefresh()
        {
            _hasAll = false;
            StateHasChanged();

            if (_instance is not null)
            {
                await _instance.InvokeVoidAsync("dispose");
                await _instance.DisposeAsync();
            }

            await RunObserver();
        }

        public async ValueTask DisposeAsync()
        {
            if (_instance is not null)
            {
                await _instance.InvokeVoidAsync("dispose");
                await _instance.DisposeAsync();
            }

            if (_module is not null)
            {
                await _module.DisposeAsync();
            }

        }
    }
}
