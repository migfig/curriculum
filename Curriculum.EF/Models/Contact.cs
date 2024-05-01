using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Contact
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[Required]
	[MaxLength(150)]
	public string? Email { get;  set; } = "Default";
	[MaxLength(20)]
	public string? Phone { get;  set; } = "Default";
	[MaxLength(20)]
	public string? Mobile { get;  set; } = "Default";
	public bool IsDeleted { get;  set; } = false;
	[Required]
	public Guid UserId { get;  set; } = Guid.NewGuid();
	public User? User { get;  set; }

    // For serialization
    public Contact() { }

    public static Contact CreateContact(
        Guid id,
		string? email,
		string? phone,
		string? mobile,
		bool isDeleted,
		Guid userId
    )
    {
        return new Contact(
            id,
			email,
			phone,
			mobile,
			isDeleted,
			userId
        );
    }

    private Contact(
        Guid id,
		string? email,
		string? phone,
		string? mobile,
		bool isDeleted,
		Guid userId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(email))
            throw new ArgumentOutOfRangeException(nameof(email));

        Id = id;
			Email = email;
			Phone = phone;
			Mobile = mobile;
			IsDeleted = isDeleted;
			UserId = userId;
    }

    public void Update(
        string? email,
		string? phone,
		string? mobile,
		bool isDeleted,
		Guid userId
    )
    {

        if (string.IsNullOrEmpty(email))
            throw new ArgumentOutOfRangeException(nameof(email));

        Email = email;
			Phone = phone;
			Mobile = mobile;
			IsDeleted = isDeleted;
			UserId = userId;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    #region expressions
    private static Expression<Func<Contact, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Contact, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Email + item.Phone + item.Mobile).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.UserId == parentId : true);
    private static Expression<Func<Contact, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Email + item.Phone + item.Mobile).Contains(searchTerm) && (parentId != null ? item.UserId == parentId : true);

    private static Func<IQueryable<Contact>, IOrderedQueryable<Contact>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Email" => items.OrderBy(item => item.Email),
			"Phone" => items.OrderBy(item => item.Phone),
			"Mobile" => items.OrderBy(item => item.Mobile),
			"IsDeleted" => items.OrderBy(item => item.IsDeleted)
        };
    private static Func<IQueryable<Contact>, IOrderedQueryable<Contact>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Email" => items.OrderByDescending(item => item.Email),
			"Phone" => items.OrderByDescending(item => item.Phone),
			"Mobile" => items.OrderByDescending(item => item.Mobile),
			"IsDeleted" => items.OrderByDescending(item => item.IsDeleted)
        };

    public static bool FilterFunc(Contact item, string searchTerm) 
        => (item.Email + item.Phone + item.Mobile).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Contact>, IOrderedQueryable<Contact>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Contact, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Contact>, IOrderedQueryable<Contact>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
