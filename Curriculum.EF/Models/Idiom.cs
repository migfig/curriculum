using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Idiom
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[Required]
	[MaxLength(50)]
	public string Name { get;  set; } = "Default";
	public string Level { get;  set; } = "Default";
	[MaxLength(50)]
	public string SchoolName { get;  set; } = "Default";
	public string Location { get;  set; } = "Default";
	[Required]
	public Guid ResumeId { get;  set; } = Guid.NewGuid();
	public Resume? Resume { get;  set; }

    // For serialization
    public Idiom() { }

    public static Idiom CreateIdiom(
        Guid id,
		string name,
		string level,
		string schoolName,
		string location,
		Guid resumeId
    )
    {
        return new Idiom(
            id,
			name,
			level,
			schoolName,
			location,
			resumeId
        );
    }

    private Idiom(
        Guid id,
		string name,
		string level,
		string schoolName,
		string location,
		Guid resumeId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));

        Id = id;
			Name = name;
			Level = level;
			SchoolName = schoolName;
			Location = location;
			ResumeId = resumeId;
    }

    public void Update(
        string name,
		string level,
		string schoolName,
		string location,
		Guid resumeId
    )
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));

        Name = name;
			Level = level;
			SchoolName = schoolName;
			Location = location;
			ResumeId = resumeId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<Idiom, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Idiom, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Level + item.SchoolName + item.Location).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.ResumeId == parentId : true);
    private static Expression<Func<Idiom, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Level + item.SchoolName + item.Location).Contains(searchTerm) && (parentId != null ? item.ResumeId == parentId : true);

    private static Func<IQueryable<Idiom>, IOrderedQueryable<Idiom>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderBy(item => item.Name),
			"Level" => items.OrderBy(item => item.Level),
			"SchoolName" => items.OrderBy(item => item.SchoolName),
			"Location" => items.OrderBy(item => item.Location)
        };
    private static Func<IQueryable<Idiom>, IOrderedQueryable<Idiom>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderByDescending(item => item.Name),
			"Level" => items.OrderByDescending(item => item.Level),
			"SchoolName" => items.OrderByDescending(item => item.SchoolName),
			"Location" => items.OrderByDescending(item => item.Location)
        };

    public static bool FilterFunc(Idiom item, string searchTerm) 
        => (item.Name + item.Level + item.SchoolName + item.Location).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Idiom>, IOrderedQueryable<Idiom>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Idiom, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Idiom>, IOrderedQueryable<Idiom>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
