using mud = MudBlazor;
using Curriculum.Blazor.Components;
using Curriculum.EF.Models;

namespace Curriculum.Blazor.Pages;

public partial class Index : BaseComponent<Resume>
{
    IReadOnlyList<SummaryResponse> Summaries = null!;

    protected override string PageName()
    {
        return "Index";
    }

    protected override string PageRoute()
    {
        return $"/";
    }

    protected override bool LoadOnInitialize()
    {
        return false;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Summaries = new List<SummaryResponse>();
    }
}
