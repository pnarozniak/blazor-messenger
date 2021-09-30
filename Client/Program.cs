using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using messanger.Client.Repositories.Implementations;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Implementations;
using messanger.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace messanger.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("messanger.ServerAPI",
                    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("messanger.ServerAPI"));

            builder.Services.AddApiAuthorization();
            builder.Logging.SetMinimumLevel(LogLevel.None);
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IConversationsRepository, ConversationsRepository>();
            services.AddScoped<IFriendshipsRepository, FriendshipsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddSingleton<IAppStateService, AppStateService>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddBlazoredSessionStorage();
        }
    }
}