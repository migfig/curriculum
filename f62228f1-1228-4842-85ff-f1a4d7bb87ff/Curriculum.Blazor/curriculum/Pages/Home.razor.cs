using Microsoft.AspNetCore.Components;
using MudBlazor;
using Curriculum.Blazor.Components;
using Curriculum.EF.Models;

namespace Curriculum.Blazor.Pages;

public partial class Home : BaseComponent<Resume>
{

    User? user;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var itemParams = GetItemParameters();
        itemParams.PageNumber = 1;
        itemParams.PageSize = 10;
        itemParams.SortBy = "Name";
        itemParams.SortDirection = eSortDirection.Ascending;
        itemParams.SearchTerm = itemsFilter;
        itemParams.SearchCase = eSearchCase.IgnoreCase;

        var pagedList = await repository.GetPagedItems(itemParams);
        itemList = pagedList.Items;

        itemParams.SearchTerm = itemList.FirstOrDefault().UserId.ToString();
        user = (await userRepository.GetItems(itemParams)).FirstOrDefault();
    }

    protected override bool LoadOnInitialize()
    {
        return false;
    }

    protected override Dictionary<string, string> Routes()
    {
        return new Dictionary<string, string>{
            { "Resume", $"/home" },
        };
    }

}
