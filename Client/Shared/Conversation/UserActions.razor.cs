using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace messanger.Client.Shared.Conversation
{
    public partial class UserActions
    {
        private string _inputValue;

        [Parameter] public EventCallback<string> OnSend { get; set; }

        private async Task OnKeyUp(KeyboardEventArgs args)
        {
            if (args.Key != "Enter") return;
            if (string.IsNullOrWhiteSpace(_inputValue)) return;

            var messageContent = _inputValue;
            _inputValue = string.Empty;
            await OnSend.InvokeAsync(messageContent);
        }
    }
}
