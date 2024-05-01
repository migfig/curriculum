namespace Curriculum.EF.Models;

public class ItemParameters
{
    const int maxPageSize = 100;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }

    public string SearchTerm { get; set; } = string.Empty;
    public eSearchCase SearchCase { get; set; } = eSearchCase.LowerCase;
    public string SortBy { get; set; } = string.Empty;
    public eSortDirection SortDirection { get; set; } = eSortDirection.Ascending;
    public Guid ParentId { get; set; } = Guid.Empty;
}

public enum eSortDirection {
    Ascending,
    Descending,
    None
}

public enum eSearchCase {
    LowerCase,
    IgnoreCase
}
