using Microsoft.AspNetCore.Components;
using Shared.Dtos;
using System.Net.Http.Json;

public class ClientsBase : ComponentBase
{
    [Inject] protected HttpClient Http { get; set; }

    protected List<ClientDto> clients = new();
    protected ClientDto editClient = new();
    protected bool showForm = false;
    protected bool isLoading = true;
    protected bool isEdit = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadClients();
    }

    protected async Task LoadClients()
    {
        isLoading = true;
        clients = await Http.GetFromJsonAsync<List<ClientDto>>("/users");
        isLoading = false;
    }

    protected void ShowAddClient()
    {
        editClient = new ClientDto();
        showForm = true;
        isEdit = false;
    }

    protected void EditClient(ClientDto client)
    {
        editClient = new ClientDto
        {
            Id = client.Id,
            NomSociete = client.NomSociete,
            Adresse = client.Adresse,
            EmailAdministrateur = client.EmailAdministrateur,
            MotDePasseAdministrateur = client.MotDePasseAdministrateur
        };
        showForm = true;
        isEdit = true;
    }

    protected async Task SaveClient()
    {
        if (isEdit)
        {
            await Http.PutAsJsonAsync($"/users/{editClient.Id}", editClient);
        }
        else
        {
            await Http.PostAsJsonAsync("/users", editClient);
        }
        showForm = false;
        await LoadClients();
    }

    protected async Task DeleteClient(int id)
    {
        await Http.DeleteAsync($"/users/{id}");
        await LoadClients();
    }

    protected void CancelEdit()
    {
        showForm = false;
    }
}

// La méthode ShowAddClient() initialise editClient et affiche le formulaire de création.
// La méthode SaveClient() gère la création ou la modification selon la valeur de isEdit.