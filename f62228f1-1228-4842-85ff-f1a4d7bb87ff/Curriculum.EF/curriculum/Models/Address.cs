using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Address
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[MaxLength(150)]
	public string? Street { get;  set; } = "Default";
	[MaxLength(50)]
	public string? ExteriorNumber { get;  set; } = "Default";
	[MaxLength(50)]
	public string? InteriorNumber { get;  set; } = "Default";
	[MaxLength(50)]
	public string? PostalCode { get;  set; } = "Default";
	[MaxLength(50)]
	public string? City { get;  set; } = "Default";
	[MaxLength(50)]
	public string? State { get;  set; } = "Default";
	[MaxLength(50)]
	public string? Country { get;  set; } = "Default";
	public bool IsDeleted { get;  set; } = false;
	public string Zip { get;  set; } = "Default";
	[Required]
	public Guid UserId { get;  set; } = Guid.NewGuid();
	public User? User { get;  set; }

    // For serialization
    public Address() { }

    public static Address CreateAddress(
        Guid id,
		string? street,
		string? exteriorNumber,
		string? interiorNumber,
		string? postalCode,
		string? city,
		string? state,
		string? country,
		bool isDeleted,
		string zip,
		Guid userId
    )
    {
        return new Address(
            id,
			street,
			exteriorNumber,
			interiorNumber,
			postalCode,
			city,
			state,
			country,
			isDeleted,
			zip,
			userId
        );
    }

    private Address(
        Guid id,
		string? street,
		string? exteriorNumber,
		string? interiorNumber,
		string? postalCode,
		string? city,
		string? state,
		string? country,
		bool isDeleted,
		string zip,
		Guid userId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));


        Id = id;
			Street = street;
			ExteriorNumber = exteriorNumber;
			InteriorNumber = interiorNumber;
			PostalCode = postalCode;
			City = city;
			State = state;
			Country = country;
			IsDeleted = isDeleted;
			Zip = zip;
			UserId = userId;
    }

    public void Update(
        string? street,
		string? exteriorNumber,
		string? interiorNumber,
		string? postalCode,
		string? city,
		string? state,
		string? country,
		bool isDeleted,
		string zip,
		Guid userId
    )
    {


        Street = street;
			ExteriorNumber = exteriorNumber;
			InteriorNumber = interiorNumber;
			PostalCode = postalCode;
			City = city;
			State = state;
			Country = country;
			IsDeleted = isDeleted;
			Zip = zip;
			UserId = userId;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    #region expressions
    private static Expression<Func<Address, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Address, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Street + item.ExteriorNumber + item.InteriorNumber + item.PostalCode + item.City + item.State + item.Country + item.Zip).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.UserId == parentId : true);
    private static Expression<Func<Address, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Street + item.ExteriorNumber + item.InteriorNumber + item.PostalCode + item.City + item.State + item.Country + item.Zip).Contains(searchTerm) && (parentId != null ? item.UserId == parentId : true);

    private static Func<IQueryable<Address>, IOrderedQueryable<Address>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Street" => items.OrderBy(item => item.Street),
			"ExteriorNumber" => items.OrderBy(item => item.ExteriorNumber),
			"InteriorNumber" => items.OrderBy(item => item.InteriorNumber),
			"PostalCode" => items.OrderBy(item => item.PostalCode),
			"City" => items.OrderBy(item => item.City),
			"State" => items.OrderBy(item => item.State),
			"Country" => items.OrderBy(item => item.Country),
			"IsDeleted" => items.OrderBy(item => item.IsDeleted),
			"Zip" => items.OrderBy(item => item.Zip)
        };
    private static Func<IQueryable<Address>, IOrderedQueryable<Address>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Street" => items.OrderByDescending(item => item.Street),
			"ExteriorNumber" => items.OrderByDescending(item => item.ExteriorNumber),
			"InteriorNumber" => items.OrderByDescending(item => item.InteriorNumber),
			"PostalCode" => items.OrderByDescending(item => item.PostalCode),
			"City" => items.OrderByDescending(item => item.City),
			"State" => items.OrderByDescending(item => item.State),
			"Country" => items.OrderByDescending(item => item.Country),
			"IsDeleted" => items.OrderByDescending(item => item.IsDeleted),
			"Zip" => items.OrderByDescending(item => item.Zip)
        };

    public static bool FilterFunc(Address item, string searchTerm) 
        => (item.Street + item.ExteriorNumber + item.InteriorNumber + item.PostalCode + item.City + item.State + item.Country + item.Zip).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Address>, IOrderedQueryable<Address>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Address, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Address>, IOrderedQueryable<Address>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
