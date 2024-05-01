using Microsoft.AspNetCore.Components;
using MudBlazor;
using Blazored.LocalStorage;
using Curriculum.EF.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Curriculum.Blazor.Components;

public abstract class BaseComponent<T> : MudComponentBase, IDisposable where T : class
{
    [Inject]
    protected IApplicationState state { get; set; }

    [Inject]
    public IHttpRepository<T> repository { get; set; }

    [Inject]
    protected ILogger<BaseComponent<T>> logger { get; set; }

    [Inject]
    protected ILocalStorageService localStorage { get; set; }

    [Inject]
    protected IDialogService dialogService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public AuthenticationStateProvider authStateProvider {get; set;}

    protected ViewMode viewMode = ViewMode.TableView;
    protected string itemsFilter = string.Empty;
    protected bool hidePagination = false;
    protected IReadOnlyList<T> itemList { get; set; } = new List<T>();

    protected override async Task OnInitializedAsync()
    {
        viewMode = await localStorage.GetItemAsync<ViewMode>(PageViewKey());
        state.OnChange += StateHasChanged;

        var isUserAuthenticated = await ((AuthStateProvider)authStateProvider).IsAuthenticatedAsync();
        if (!isUserAuthenticated) {
            NavigationManager.NavigateTo("/login");
            return;
        }
        else {
            NavigationManager.NavigateTo("/home");
        }

        var i = 0;
        state.PushBreadcrumItem(
            true,
            Routes().ToArray()
                .Select(r => new { idx = i++, item = new BreadcrumbItem(r.Key, r.Value, i == Routes().Count) })
                .OrderByDescending(r => r.idx)
                .Select(r => r.item)
                .ToArray()
        );
        if (this.LoadOnInitialize()) {
            itemList = await repository.GetItems(GetItemParameters());
        }
    }

    public void Dispose()
    {
        state.OnChange -= StateHasChanged;
    }

    protected async Task ToogleViewMode()
    {
        viewMode = viewMode == ViewMode.TableView ? ViewMode.CardView : ViewMode.TableView;
        await localStorage.SetItemAsync(PageViewKey(), viewMode);
    }

    protected virtual void editSettings(Guid id)
    {
        var item = itemList.First(a => a.EqualsId(id));
        state.SetActiveItem(item.GetType().Name, item);
        state.SetCurrentRoute(this.PageRoute());
        state.ToggleToolBox();
    }

    protected async Task deleteItem(Guid id)
    {
        var item = itemList.First(a => a.EqualsId(id));
        var parameters = new DialogParameters();
        parameters.Add("ContentText", $"Are you sure want to delete the {item.GetType().Name} ({item.NameValue()})? This process cannot be undone.");
        parameters.Add("ButtonText", "Delete");
        parameters.Add("Color", Color.Error);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = dialogService.Show<ConfirmationDialog>("Delete Confirmation", parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await repository.DeleteItem(id);
            itemList = new List<T>(itemList.Where(i => i.EqualsId(id)));
            StateHasChanged();
        }
    }

    protected virtual Task AddItem()
    {
        return Task.Delay(50);
    }

    protected virtual string PageRoute()
    {
        return string.Empty;
    }

    protected virtual string PageName()
    {
        return string.Empty;
    }

    protected virtual string PageViewKey()
    {
        return $"{this.PageName()}-viewMode";
    }

    protected virtual bool IsBaseClass()
    {
        return false;
    }

    protected virtual bool LoadOnInitialize()
    {
        return true;
    }

    protected virtual Dictionary<string, string> Routes()
    {
        return new Dictionary<string, string>();
    }

    protected virtual ItemParameters GetItemParameters()
    {
        return new ItemParameters { };
    }
}