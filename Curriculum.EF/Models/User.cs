using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class User
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[Required]
	[MaxLength(50)]
	public string? Name { get;  set; } = "Default";
	[Required]
	[MaxLength(150)]
	public string? Email { get;  set; } = "Default";
	[MaxLength(20)]
	public string? Phone { get;  set; } = "Default";
	[MaxLength(50)]
	public string? JobTitle { get;  set; } = "Default";
	[Required]
	[MaxLength(50)]
	public string? FirstName { get;  set; } = "Default";
	[Required]
	[MaxLength(50)]
	public string? LastName { get;  set; } = "Default";
	[MaxLength(1024)]
	public string? Avatar { get;  set; } = "Default";
	public bool IsDeleted { get;  set; } = false;
	public DateTime? DateCreated { get;  set; } = DateTime.Now.ToUniversalTime();
	public Guid? CreatedBy { get;  set; } = Guid.NewGuid();
	public DateTime? DateUpdated { get;  set; } = DateTime.Now.ToUniversalTime();
	public Guid? UpdatedBy { get;  set; } = Guid.NewGuid();
	[MaxLength(512)]
	public string? PwdHash { get;  set; } = "Default";
	[MaxLength(512)]
	public string? PwdSalt { get;  set; } = "Default";
	public ICollection<Address> Addresses { get;  set; } = new List<Address>();
	public ICollection<Contact> Contacts { get;  set; } = new List<Contact>();
	public ICollection<Group> Groups { get;  set; } = new List<Group>();

    // For serialization
    public User() { }

    public static User CreateUser(
        Guid id,
		string? name,
		string? email,
		string? phone,
		string? jobTitle,
		string? firstName,
		string? lastName,
		string? avatar,
		bool isDeleted,
		DateTime? dateCreated,
		Guid? createdBy,
		DateTime? dateUpdated,
		Guid? updatedBy,
		string? pwdHash,
		string? pwdSalt,
		ICollection<Address> addresses,
		ICollection<Contact> contacts,
		ICollection<Group> groups
    )
    {
        return new User(
            id,
			name,
			email,
			phone,
			jobTitle,
			firstName,
			lastName,
			avatar,
			isDeleted,
			dateCreated,
			createdBy,
			dateUpdated,
			updatedBy,
			pwdHash,
			pwdSalt,
			addresses,
			contacts,
			groups
        );
    }

    private User(
        Guid id,
		string? name,
		string? email,
		string? phone,
		string? jobTitle,
		string? firstName,
		string? lastName,
		string? avatar,
		bool isDeleted,
		DateTime? dateCreated,
		Guid? createdBy,
		DateTime? dateUpdated,
		Guid? updatedBy,
		string? pwdHash,
		string? pwdSalt,
		ICollection<Address> addresses,
		ICollection<Contact> contacts,
		ICollection<Group> groups
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));
        if (string.IsNullOrEmpty(email))
            throw new ArgumentOutOfRangeException(nameof(email));
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentOutOfRangeException(nameof(firstName));
        if (string.IsNullOrEmpty(lastName))
            throw new ArgumentOutOfRangeException(nameof(lastName));

        if (!createdBy.HasValue)
            throw new ArgumentNullException(nameof(createdBy));

        if (!updatedBy.HasValue)
            throw new ArgumentNullException(nameof(updatedBy));

        Id = id;
			Name = name;
			Email = email;
			Phone = phone;
			JobTitle = jobTitle;
			FirstName = firstName;
			LastName = lastName;
			Avatar = avatar;
			IsDeleted = isDeleted;
			DateCreated = dateCreated;
			CreatedBy = createdBy;
			DateUpdated = dateUpdated;
			UpdatedBy = updatedBy;
			PwdHash = pwdHash;
			PwdSalt = pwdSalt;
			Addresses = addresses;
			Contacts = contacts;
			Groups = groups;
    }

    public void Update(
        string? name,
		string? email,
		string? phone,
		string? jobTitle,
		string? firstName,
		string? lastName,
		string? avatar,
		bool isDeleted,
		DateTime? dateCreated,
		Guid? createdBy,
		DateTime? dateUpdated,
		Guid? updatedBy,
		string? pwdHash,
		string? pwdSalt,
		ICollection<Address> addresses,
		ICollection<Contact> contacts,
		ICollection<Group> groups
    )
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));
        if (string.IsNullOrEmpty(email))
            throw new ArgumentOutOfRangeException(nameof(email));
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentOutOfRangeException(nameof(firstName));
        if (string.IsNullOrEmpty(lastName))
            throw new ArgumentOutOfRangeException(nameof(lastName));

        if (!createdBy.HasValue)
            throw new ArgumentNullException(nameof(createdBy));

        if (!updatedBy.HasValue)
            throw new ArgumentNullException(nameof(updatedBy));
        Name = name;
			Email = email;
			Phone = phone;
			JobTitle = jobTitle;
			FirstName = firstName;
			LastName = lastName;
			Avatar = avatar;
			IsDeleted = isDeleted;
			DateCreated = dateCreated;
			CreatedBy = createdBy;
			DateUpdated = dateUpdated;
			UpdatedBy = updatedBy;
			PwdHash = pwdHash;
			PwdSalt = pwdSalt;
			Addresses = addresses;
			Contacts = contacts;
			Groups = groups;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    #region expressions
    private static Expression<Func<User, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<User, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Email + item.Phone + item.JobTitle + item.FirstName + item.LastName + item.Avatar + item.PwdHash + item.PwdSalt).ToLower().Contains(searchTerm.ToLower()) ;
    private static Expression<Func<User, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Email + item.Phone + item.JobTitle + item.FirstName + item.LastName + item.Avatar + item.PwdHash + item.PwdSalt).Contains(searchTerm) ;

    private static Func<IQueryable<User>, IOrderedQueryable<User>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderBy(item => item.Name),
			"Email" => items.OrderBy(item => item.Email),
			"Phone" => items.OrderBy(item => item.Phone),
			"JobTitle" => items.OrderBy(item => item.JobTitle),
			"FirstName" => items.OrderBy(item => item.FirstName),
			"LastName" => items.OrderBy(item => item.LastName),
			"Avatar" => items.OrderBy(item => item.Avatar),
			"IsDeleted" => items.OrderBy(item => item.IsDeleted),
			"DateCreated" => items.OrderBy(item => item.DateCreated),
			"DateUpdated" => items.OrderBy(item => item.DateUpdated),
			"PwdHash" => items.OrderBy(item => item.PwdHash),
			"PwdSalt" => items.OrderBy(item => item.PwdSalt)
        };
    private static Func<IQueryable<User>, IOrderedQueryable<User>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderByDescending(item => item.Name),
			"Email" => items.OrderByDescending(item => item.Email),
			"Phone" => items.OrderByDescending(item => item.Phone),
			"JobTitle" => items.OrderByDescending(item => item.JobTitle),
			"FirstName" => items.OrderByDescending(item => item.FirstName),
			"LastName" => items.OrderByDescending(item => item.LastName),
			"Avatar" => items.OrderByDescending(item => item.Avatar),
			"IsDeleted" => items.OrderByDescending(item => item.IsDeleted),
			"DateCreated" => items.OrderByDescending(item => item.DateCreated),
			"DateUpdated" => items.OrderByDescending(item => item.DateUpdated),
			"PwdHash" => items.OrderByDescending(item => item.PwdHash),
			"PwdSalt" => items.OrderByDescending(item => item.PwdSalt)
        };

    public static bool FilterFunc(User item, string searchTerm) 
        => (item.Name + item.Email + item.Phone + item.JobTitle + item.FirstName + item.LastName + item.Avatar + item.PwdHash + item.PwdSalt).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<User>, IOrderedQueryable<User>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<User, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<User>, IOrderedQueryable<User>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
