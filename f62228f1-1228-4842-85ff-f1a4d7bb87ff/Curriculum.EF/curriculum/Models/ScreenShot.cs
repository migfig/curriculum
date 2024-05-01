using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class ScreenShot
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[Required]
	[MaxLength(50)]
	public string Name { get;  set; } = "Default";
	public string Url { get;  set; } = "Default";
	[Required]
	public Guid ProjectId { get;  set; } = Guid.NewGuid();
	public Project? Project { get;  set; }

    // For serialization
    public ScreenShot() { }

    public static ScreenShot CreateScreenShot(
        Guid id,
		string name,
		string url,
		Guid projectId
    )
    {
        return new ScreenShot(
            id,
			name,
			url,
			projectId
        );
    }

    private ScreenShot(
        Guid id,
		string name,
		string url,
		Guid projectId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));
        Id = id;
			Name = name;
			Url = url;
			ProjectId = projectId;
    }

    public void Update(
        string name,
		string url,
		Guid projectId
    )
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));

        Name = name;
			Url = url;
			ProjectId = projectId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<ScreenShot, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<ScreenShot, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Url).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.ProjectId == parentId : true);
    private static Expression<Func<ScreenShot, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Url).Contains(searchTerm) && (parentId != null ? item.ProjectId == parentId : true);

    private static Func<IQueryable<ScreenShot>, IOrderedQueryable<ScreenShot>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderBy(item => item.Name),
			"Url" => items.OrderBy(item => item.Url)
        };
    private static Func<IQueryable<ScreenShot>, IOrderedQueryable<ScreenShot>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderByDescending(item => item.Name),
			"Url" => items.OrderByDescending(item => item.Url)
        };

    public static bool FilterFunc(ScreenShot item, string searchTerm) 
        => (item.Name + item.Url).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<ScreenShot>, IOrderedQueryable<ScreenShot>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<ScreenShot, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
        if (string.IsNullOrEmpty(searchTerm) && parentId == null) return null;

        Guid id;
		if (!string.IsNullOrEmpty(searchTerm) && searchTerm.Length == 36 && Guid.TryParse(searchTerm, out id)) {
			return GuidFilter(id);
		}

        return searchCase switch {
            eSearchCase.IgnoreCase => DefaultIgnoreCaseFilter(searchTerm, parentId),
            eSearchCase.LowerCase => DefaultLowerCaseFilter(searchTerm, parentId),
        };
    }

    public static Func<IQueryable<ScreenShot>, IOrderedQueryable<ScreenShot>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
