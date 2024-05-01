using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Project
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[MaxLength(1024)]
	public string Description { get;  set; } = "Default";
	public bool IsStack { get;  set; } = false;
	public DateTime StartDate { get;  set; } = DateTime.Now.ToUniversalTime();
	public DateTime EndDate { get;  set; } = DateTime.Now.ToUniversalTime();
	public string[] UsedTools { get;  set; } = new string[] {};
	public ICollection<ScreenShot> ScreenShots { get;  set; } = new List<ScreenShot>();
	[Required]
	public Guid ResumeId { get;  set; } = Guid.NewGuid();
	public Resume? Resume { get;  set; }

    // For serialization
    public Project() { }

    public static Project CreateProject(
        Guid id,
		string description,
		bool isStack,
		DateTime startDate,
		DateTime endDate,
		string[] usedTools,
		ICollection<ScreenShot> screenShots,
		Guid resumeId
    )
    {
        return new Project(
            id,
			description,
			isStack,
			startDate,
			endDate,
			usedTools,
			screenShots,
			resumeId
        );
    }

    private Project(
        Guid id,
		string description,
		bool isStack,
		DateTime startDate,
		DateTime endDate,
		string[] usedTools,
		ICollection<ScreenShot> screenShots,
		Guid resumeId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));


        Id = id;
			Description = description;
			IsStack = isStack;
			StartDate = startDate;
			EndDate = endDate;
			UsedTools = usedTools;
			ScreenShots = screenShots;
			ResumeId = resumeId;
    }

    public void Update(
        string description,
		bool isStack,
		DateTime startDate,
		DateTime endDate,
		string[] usedTools,
		ICollection<ScreenShot> screenShots,
		Guid resumeId
    )
    {

        Description = description;
			IsStack = isStack;
			StartDate = startDate;
			EndDate = endDate;
			UsedTools = usedTools;
			ScreenShots = screenShots;
			ResumeId = resumeId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<Project, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Project, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Description).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.ResumeId == parentId : true);
    private static Expression<Func<Project, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Description).Contains(searchTerm) && (parentId != null ? item.ResumeId == parentId : true);

    private static Func<IQueryable<Project>, IOrderedQueryable<Project>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Description" => items.OrderBy(item => item.Description),
			"IsStack" => items.OrderBy(item => item.IsStack),
			"StartDate" => items.OrderBy(item => item.StartDate),
			"EndDate" => items.OrderBy(item => item.EndDate)
        };
    private static Func<IQueryable<Project>, IOrderedQueryable<Project>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Description" => items.OrderByDescending(item => item.Description),
			"IsStack" => items.OrderByDescending(item => item.IsStack),
			"StartDate" => items.OrderByDescending(item => item.StartDate),
			"EndDate" => items.OrderByDescending(item => item.EndDate)
        };

    public static bool FilterFunc(Project item, string searchTerm) 
        => (item.Description).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Project>, IOrderedQueryable<Project>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Project, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Project>, IOrderedQueryable<Project>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
