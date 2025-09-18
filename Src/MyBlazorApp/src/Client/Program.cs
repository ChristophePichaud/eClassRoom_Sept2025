using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace YourNamespace
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => {
                var navigationManager = sp.GetRequiredService<NavigationManager>();
                var js = sp.GetRequiredService<IJSRuntime>();
                var httpClient = new HttpClient(new AuthMessageHandler(js)) { BaseAddress = new Uri(navigationManager.BaseUri) };
                return httpClient;
            });

            await builder.Build().RunAsync();
        }
    }

    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;
        public AuthMessageHandler(IJSRuntime js)
        {
            _js = js;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}