using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Certification
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[MaxLength(256)]
	public string Title { get;  set; } = "Default";
	[MaxLength(50)]
	public string SchoolName { get;  set; } = "Default";
	public string Location { get;  set; } = "Default";
	public bool IsEducation { get;  set; } = false;
	public DateTime StartDate { get;  set; } = DateTime.Now.ToUniversalTime();
	public DateTime EndDate { get;  set; } = DateTime.Now.ToUniversalTime();
	public DateTime ExpireDate { get;  set; } = DateTime.Now.ToUniversalTime();
	[Required]
	public Guid ResumeId { get;  set; } = Guid.NewGuid();
	public Resume? Resume { get;  set; }

    // For serialization
    public Certification() { }

    public static Certification CreateCertification(
        Guid id,
		string title,
		string schoolName,
		string location,
		bool isEducation,
		DateTime startDate,
		DateTime endDate,
		DateTime expireDate,
		Guid resumeId
    )
    {
        return new Certification(
            id,
			title,
			schoolName,
			location,
			isEducation,
			startDate,
			endDate,
			expireDate,
			resumeId
        );
    }

    private Certification(
        Guid id,
		string title,
		string schoolName,
		string location,
		bool isEducation,
		DateTime startDate,
		DateTime endDate,
		DateTime expireDate,
		Guid resumeId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        Id = id;
			Title = title;
			SchoolName = schoolName;
			Location = location;
			IsEducation = isEducation;
			StartDate = startDate;
			EndDate = endDate;
			ExpireDate = expireDate;
			ResumeId = resumeId;
    }

    public void Update(
        string title,
		string schoolName,
		string location,
		bool isEducation,
		DateTime startDate,
		DateTime endDate,
		DateTime expireDate,
		Guid resumeId
    )
    {
        Title = title;
			SchoolName = schoolName;
			Location = location;
			IsEducation = isEducation;
			StartDate = startDate;
			EndDate = endDate;
			ExpireDate = expireDate;
			ResumeId = resumeId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<Certification, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Certification, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Title + item.SchoolName + item.Location).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.ResumeId == parentId : true);
    private static Expression<Func<Certification, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Title + item.SchoolName + item.Location).Contains(searchTerm) && (parentId != null ? item.ResumeId == parentId : true);

    private static Func<IQueryable<Certification>, IOrderedQueryable<Certification>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Title" => items.OrderBy(item => item.Title),
			"SchoolName" => items.OrderBy(item => item.SchoolName),
			"Location" => items.OrderBy(item => item.Location),
			"IsEducation" => items.OrderBy(item => item.IsEducation),
			"StartDate" => items.OrderBy(item => item.StartDate),
			"EndDate" => items.OrderBy(item => item.EndDate),
			"ExpireDate" => items.OrderBy(item => item.ExpireDate)
        };
    private static Func<IQueryable<Certification>, IOrderedQueryable<Certification>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Title" => items.OrderByDescending(item => item.Title),
			"SchoolName" => items.OrderByDescending(item => item.SchoolName),
			"Location" => items.OrderByDescending(item => item.Location),
			"IsEducation" => items.OrderByDescending(item => item.IsEducation),
			"StartDate" => items.OrderByDescending(item => item.StartDate),
			"EndDate" => items.OrderByDescending(item => item.EndDate),
			"ExpireDate" => items.OrderByDescending(item => item.ExpireDate)
        };

    public static bool FilterFunc(Certification item, string searchTerm) 
        => (item.Title + item.SchoolName + item.Location).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Certification>, IOrderedQueryable<Certification>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Certification, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Certification>, IOrderedQueryable<Certification>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
