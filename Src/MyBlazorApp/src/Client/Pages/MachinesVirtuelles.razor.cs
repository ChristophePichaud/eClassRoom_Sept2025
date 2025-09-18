using Microsoft.AspNetCore.Components;
using Shared.Dtos;
using System.Net.Http.Json;

public class MachinesVirtuellesBase : ComponentBase
{
    [Inject] protected HttpClient Http { get; set; }

    protected List<MachineVirtuelleDto> machines = new();
    protected MachineVirtuelleDto editVm = new();
    protected bool showForm = false;
    protected bool isLoading = true;
    protected bool isEdit = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadMachines();
    }

    protected async Task LoadMachines()
    {
        isLoading = true;
        machines = await Http.GetFromJsonAsync<List<MachineVirtuelleDto>>("/machines");
        isLoading = false;
    }

    protected void ShowAddVm()
    {
        editVm = new MachineVirtuelleDto();
        showForm = true;
        isEdit = false;
    }

    protected void EditVm(MachineVirtuelleDto vm)
    {
        editVm = new MachineVirtuelleDto
        {
            Id = vm.Id,
            Nom = vm.Nom,
            TypeOs = vm.TypeOs,
            TypeVm = vm.TypeVm,
            Sku = vm.Sku,
            Offer = vm.Offer,
            Version = vm.Version,
            DiskIso = vm.DiskIso,
            NomMarketing = vm.NomMarketing
        };
        showForm = true;
        isEdit = true;
    }

    protected async Task SaveVm()
    {
        if (isEdit)
        {
            await Http.PutAsJsonAsync($"/machines/{editVm.Id}", editVm);
        }
        else
        {
            await Http.PostAsJsonAsync("/machines", editVm);
        }
        showForm = false;
        await LoadMachines();
    }

    protected async Task DeleteVm(int id)
    {
        await Http.DeleteAsync($"/machines/{id}");
        await LoadMachines();
    }

    protected void CancelEdit()
    {
        showForm = false;
    }
}
