using System.Net.Http.Json;
using Microsoft.JSInterop;

public partial class Login
{
    private string username;
    private string password;
    private string errorMessage;

    private async Task HandleLogin()
    {
        var response = await Http.PostAsJsonAsync("api/auth/login", new { Username = username, Password = password });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            await JS.InvokeVoidAsync("localStorage.setItem", "authToken", result.token);
            // Rediriger ou rafraîchir l’état d’authentification
        }
        else
        {
            errorMessage = "Identifiants invalides";
        }
    }

    private class TokenResponse
    {
        public string token { get; set; }
    }
}