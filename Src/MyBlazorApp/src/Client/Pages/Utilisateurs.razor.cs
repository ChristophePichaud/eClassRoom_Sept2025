using Microsoft.AspNetCore.Components;
using Shared.Dtos;
using System.Net.Http.Json;

public class UtilisateursBase : ComponentBase
{
    [Inject] protected HttpClient Http { get; set; }

    protected List<UtilisateurDto> utilisateurs = new();
    protected UtilisateurDto editUtilisateur = new();
    protected List<ClientDto> clients = new();
    protected bool showForm = false;
    protected bool isLoading = true;
    protected bool isEdit = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadClients();
        await LoadUtilisateurs();
    }

    protected async Task LoadClients()
    {
        clients = await Http.GetFromJsonAsync<List<ClientDto>>("/clients");
    }

    protected async Task LoadUtilisateurs()
    {
        isLoading = true;
        utilisateurs = await Http.GetFromJsonAsync<List<UtilisateurDto>>("/users");
        isLoading = false;
    }

    protected void ShowAddUtilisateur()
    {
        editUtilisateur = new UtilisateurDto();
        showForm = true;
        isEdit = false;
    }

    protected void EditUtilisateur(UtilisateurDto user)
    {
        editUtilisateur = new UtilisateurDto
        {
            Id = user.Id,
            Email = user.Email,
            Nom = user.Nom,
            Prenom = user.Prenom,
            MotDePasse = user.MotDePasse,
            Role = user.Role,
            ClientId = user.ClientId
        };
        showForm = true;
        isEdit = true;
    }

    protected async Task SaveUtilisateur()
    {
        if (isEdit)
        {
            await Http.PutAsJsonAsync($"/users/{editUtilisateur.Id}", editUtilisateur);
        }
        else
        {
            await Http.PostAsJsonAsync("/users", editUtilisateur);
        }
        showForm = false;
        await LoadUtilisateurs();
    }

    protected async Task DeleteUtilisateur(int id)
    {
        await Http.DeleteAsync($"/users/{id}");
        await LoadUtilisateurs();
    }

    protected void CancelEdit()
    {
        showForm = false;
    }
}
