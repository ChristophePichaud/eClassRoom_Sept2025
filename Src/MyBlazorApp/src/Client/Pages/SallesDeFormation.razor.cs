using Microsoft.AspNetCore.Components;
using Shared.Dtos;
using System.Net.Http.Json;

public class SallesDeFormationBase : ComponentBase
{
    [Inject] protected HttpClient Http { get; set; }

    protected List<SalleDeFormationDto> salles = new();
    protected SalleDeFormationDto editSalle = new();
    protected bool showForm = false;
    protected bool isLoading = true;
    protected bool isEdit = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadSalles();
    }

    protected async Task LoadSalles()
    {
        isLoading = true;
        salles = await Http.GetFromJsonAsync<List<SalleDeFormationDto>>("/salles");
        isLoading = false;
    }

    protected void ShowAddSalle()
    {
        editSalle = new SalleDeFormationDto();
        showForm = true;
        isEdit = false;
    }

    protected void EditSalle(SalleDeFormationDto salle)
    {
        editSalle = new SalleDeFormationDto
        {
            Id = salle.Id,
            Nom = salle.Nom,
            Formateur = salle.Formateur,
            DateDebut = salle.DateDebut,
            DateFin = salle.DateFin
        };
        showForm = true;
        isEdit = true;
    }

    protected async Task SaveSalle()
    {
        if (isEdit)
        {
            await Http.PutAsJsonAsync($"/salles/{editSalle.Id}", editSalle);
        }
        else
        {
            await Http.PostAsJsonAsync("/salles", editSalle);
        }
        showForm = false;
        await LoadSalles();
    }

    protected async Task DeleteSalle(int id)
    {
        await Http.DeleteAsync($"/salles/{id}");
        await LoadSalles();
    }

    protected void CancelEdit()
    {
        showForm = false;
    }
}
