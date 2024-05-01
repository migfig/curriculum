using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Permission
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[Required]
	[MaxLength(50)]
	public string? Name { get;  set; } = "Default";
	[Required]
	public Guid GroupId { get;  set; } = Guid.NewGuid();
	public Group? Group { get;  set; }

    // For serialization
    public Permission() { }

    public static Permission CreatePermission(
        Guid id,
		string? name,
		Guid groupId
    )
    {
        return new Permission(
            id,
			name,
			groupId
        );
    }

    private Permission(
        Guid id,
		string? name,
		Guid groupId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));

        Id = id;
			Name = name;
			GroupId = groupId;
    }

    public void Update(
        string? name,
		Guid groupId
    )
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));
        Name = name;
			GroupId = groupId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<Permission, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Permission, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.GroupId == parentId : true);
    private static Expression<Func<Permission, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name).Contains(searchTerm) && (parentId != null ? item.GroupId == parentId : true);

    private static Func<IQueryable<Permission>, IOrderedQueryable<Permission>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderBy(item => item.Name)
        };
    private static Func<IQueryable<Permission>, IOrderedQueryable<Permission>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderByDescending(item => item.Name)
        };

    public static bool FilterFunc(Permission item, string searchTerm) 
        => (item.Name).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Permission>, IOrderedQueryable<Permission>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Permission, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Permission>, IOrderedQueryable<Permission>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
