using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Group
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[Required]
	[MaxLength(50)]
	public string? Name { get;  set; } = "Default";
	[Required]
	public ICollection<Permission> Permissions { get;  set; } = new List<Permission>();
	[Required]
	public Guid UserId { get;  set; } = Guid.NewGuid();
	public User? User { get;  set; }

    // For serialization
    public Group() { }

    public static Group CreateGroup(
        Guid id,
		string? name,
		ICollection<Permission> permissions,
		Guid userId
    )
    {
        return new Group(
            id,
			name,
			permissions,
			userId
        );
    }

    private Group(
        Guid id,
		string? name,
		ICollection<Permission> permissions,
		Guid userId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));
        Id = id;
			Name = name;
			Permissions = permissions;
			UserId = userId;
    }

    public void Update(
        string? name,
		ICollection<Permission> permissions,
		Guid userId
    )
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));

        Name = name;
			Permissions = permissions;
			UserId = userId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<Group, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Group, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.UserId == parentId : true);
    private static Expression<Func<Group, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name).Contains(searchTerm) && (parentId != null ? item.UserId == parentId : true);

    private static Func<IQueryable<Group>, IOrderedQueryable<Group>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderBy(item => item.Name)
        };
    private static Func<IQueryable<Group>, IOrderedQueryable<Group>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderByDescending(item => item.Name)
        };

    public static bool FilterFunc(Group item, string searchTerm) 
        => (item.Name).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Group>, IOrderedQueryable<Group>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Group, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Group>, IOrderedQueryable<Group>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
