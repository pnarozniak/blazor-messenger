using Microsoft.AspNetCore.Components;

namespace messanger.Client.Shared.Conversation
{
    public partial class ConversationInfoRow
    {
        [Parameter] public string Name { get; set; }
        [Parameter] public string Avatar { get; set; } = string.Empty;
        [Parameter] public RenderFragment RightExtraContent { get; set; }
    }
}
