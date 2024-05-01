using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class StackExperience
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[MaxLength(1024)]
	public string Description { get;  set; } = "Default";
	public int Ordinal { get;  set; } = 0;
	public string[] Tags { get;  set; } = new string[] {};
	[Required]
	public Guid ResumeId { get;  set; } = Guid.NewGuid();
	public Resume? Resume { get;  set; }

    // For serialization
    public StackExperience() { }

    public static StackExperience CreateStackExperience(
        Guid id,
		string description,
        int ordinal,
		string[] tags,
		Guid resumeId
    )
    {
        return new StackExperience(
            id,
			description,
            ordinal,
			tags,
			resumeId
        );
    }

    private StackExperience(
        Guid id,
		string description,
        int ordinal,
		string[] tags,
		Guid resumeId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));

        Id = id;
			Description = description;
            Ordinal = ordinal;
			Tags = tags;
			ResumeId = resumeId;
    }

    public void Update(
        string description,
        int ordinal,
		string[] tags,
		Guid resumeId
    )
    {
        Description = description;
        Ordinal = ordinal;
			Tags = tags;
			ResumeId = resumeId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<StackExperience, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<StackExperience, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Description).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.ResumeId == parentId : true);
    private static Expression<Func<StackExperience, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Description).Contains(searchTerm) && (parentId != null ? item.ResumeId == parentId : true);

    private static Func<IQueryable<StackExperience>, IOrderedQueryable<StackExperience>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Description" => items.OrderBy(item => item.Description),
			"Ordinal" => items.OrderBy(item => item.Ordinal)
        };
    private static Func<IQueryable<StackExperience>, IOrderedQueryable<StackExperience>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Description" => items.OrderByDescending(item => item.Description),
			"Ordinal" => items.OrderByDescending(item => item.Ordinal)
        };

    public static bool FilterFunc(StackExperience item, string searchTerm) 
        => (item.Description).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<StackExperience>, IOrderedQueryable<StackExperience>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<StackExperience, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<StackExperience>, IOrderedQueryable<StackExperience>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
