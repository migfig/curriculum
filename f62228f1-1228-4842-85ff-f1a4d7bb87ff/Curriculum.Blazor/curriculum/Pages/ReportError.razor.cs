using Microsoft.AspNetCore.Components;

namespace Curriculum.Blazor.Pages
{
    public partial class ReportError
    {
        [Parameter]
        public int ErrorCode { get; set; }
        [Parameter]
        public string ErrorDescription { get; set; }
    }
}
