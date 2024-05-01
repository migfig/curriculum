using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Skill
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	
    [Required]
	[MaxLength(50)]
	public string Name { get;  set; } = "Default";
	public string Stack { get;  set; } = "Default";
	public string Level { get;  set; } = "Default";
	public double Percentage { get;  set; } = 0.0;
	public int Ordinal { get;  set; } = 0;
	public bool IsSoft { get;  set; } = false;

	[Required]
	public Guid ResumeId { get;  set; } = Guid.NewGuid();
	public Resume? Resume { get;  set; }

    // For serialization
    public Skill() { }

    public static Skill CreateSkill(
        Guid id,
		string name,
		string stack,
		string level,
		double percentage,
        int ordinal,
        bool isSoft,
		Guid resumeId
    )
    {
        return new Skill(
            id,
			name,
			stack,
			level,
			percentage,
            ordinal,
            isSoft,
			resumeId
        );
    }

    private Skill(
        Guid id,
		string name,
		string stack,
		string level,
		double percentage,
        int ordinal,
        bool isSoft,
		Guid resumeId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));

        Id = id;
        Name = name;
        Stack = stack;
        Level = level;
        Percentage = percentage;
        Ordinal = ordinal;
        IsSoft = isSoft;
        ResumeId = resumeId;
    }

    public void Update(
        string name,
		string stack,
		string level,
		double percentage,
        int ordinal,
        bool isSoft,
		Guid resumeId
    )
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));

        Name = name;
        Stack = stack;
        Level = level;
        Percentage = percentage;
        Ordinal = ordinal;
        IsSoft = isSoft;
		ResumeId = resumeId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<Skill, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Skill, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Stack + item.Level).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.ResumeId == parentId : true);
    private static Expression<Func<Skill, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Stack + item.Level).Contains(searchTerm) && (parentId != null ? item.ResumeId == parentId : true);

    private static Func<IQueryable<Skill>, IOrderedQueryable<Skill>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderBy(item => item.Name),
			"Stack" => items.OrderBy(item => item.Stack),
			"Level" => items.OrderBy(item => item.Level),
			"Percentage" => items.OrderBy(item => item.Percentage)
        };
    private static Func<IQueryable<Skill>, IOrderedQueryable<Skill>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderByDescending(item => item.Name),
			"Stack" => items.OrderByDescending(item => item.Stack),
			"Level" => items.OrderByDescending(item => item.Level),
			"Percentage" => items.OrderByDescending(item => item.Percentage)
        };

    public static bool FilterFunc(Skill item, string searchTerm) 
        => (item.Name + item.Stack + item.Level).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Skill>, IOrderedQueryable<Skill>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Skill, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Skill>, IOrderedQueryable<Skill>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
