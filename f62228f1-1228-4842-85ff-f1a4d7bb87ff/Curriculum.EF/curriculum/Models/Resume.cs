using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class Resume
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	[Required]
	[MaxLength(50)]
	public string Name { get;  set; } = "Default";
	[MaxLength(50)]
	public string Title { get;  set; } = "Default";
	public string Photo { get;  set; } = "Default";
	public eStatus Status { get;  set; } = eStatus.Default;
	public DateTime DateOfBirth { get;  set; } = DateTime.Now.ToUniversalTime();
	public string ProfileValue { get;  set; } = "Default";
	[MaxLength(50)]
	public string[] Emails { get;  set; } = new string[] {};
	[MaxLength(20)]
	public string[] Phones { get;  set; } = new string[] {};
	public Guid UserId { get;  set; } = Guid.NewGuid();
	public ICollection<StackExperience> StackExperiences { get;  set; } = new List<StackExperience>();
	public ICollection<JobExperience> JobExperiences { get;  set; } = new List<JobExperience>();
	public ICollection<Certification> Certifications { get;  set; } = new List<Certification>();
	public ICollection<Idiom> Idioms { get;  set; } = new List<Idiom>();
	public ICollection<Project> Projects { get;  set; } = new List<Project>();
	public ICollection<Skill> Skills { get;  set; } = new List<Skill>();
	public string[] Hobies { get;  set; } = new string[] {};

    // For serialization
    public Resume() { }

    public static Resume CreateResume(
        Guid id,
		string name,
		string title,
		string photo,
		eStatus status,
		DateTime dateOfBirth,
		string profileValue,
		string[] emails,
		string[] phones,
		Guid userId,
		ICollection<StackExperience> stackExperiences,
		ICollection<JobExperience> jobExperiences,
		ICollection<Certification> certifications,
		ICollection<Idiom> idioms,
		ICollection<Project> projects,
		ICollection<Skill> skills,
		string[] hobies
    )
    {
        return new Resume(
            id,
			name,
			title,
			photo,
			status,
			dateOfBirth,
			profileValue,
			emails,
			phones,
			userId,
			stackExperiences,
			jobExperiences,
			certifications,
			idioms,
			projects,
			skills,
			hobies
        );
    }

    private Resume(
        Guid id,
		string name,
		string title,
		string photo,
		eStatus status,
		DateTime dateOfBirth,
		string profileValue,
		string[] emails,
		string[] phones,
		Guid userId,
		ICollection<StackExperience> stackExperiences,
		ICollection<JobExperience> jobExperiences,
		ICollection<Certification> certifications,
		ICollection<Idiom> idioms,
		ICollection<Project> projects,
		ICollection<Skill> skills,
		string[] hobies
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));


        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId));
        Id = id;
			Name = name;
			Title = title;
			Photo = photo;
			Status = status;
			DateOfBirth = dateOfBirth;
			ProfileValue = profileValue;
			Emails = emails;
			Phones = phones;
			UserId = userId;
			StackExperiences = stackExperiences;
			JobExperiences = jobExperiences;
			Certifications = certifications;
			Idioms = idioms;
			Projects = projects;
			Skills = skills;
			Hobies = hobies;
    }

    public void Update(
        string name,
		string title,
		string photo,
		eStatus status,
		DateTime dateOfBirth,
		string profileValue,
		string[] emails,
		string[] phones,
		Guid userId,
		ICollection<StackExperience> stackExperiences,
		ICollection<JobExperience> jobExperiences,
		ICollection<Certification> certifications,
		ICollection<Idiom> idioms,
		ICollection<Project> projects,
		ICollection<Skill> skills,
		string[] hobies
    )
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentOutOfRangeException(nameof(name));


        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId));


        Name = name;
			Title = title;
			Photo = photo;
			Status = status;
			DateOfBirth = dateOfBirth;
			ProfileValue = profileValue;
			Emails = emails;
			Phones = phones;
			UserId = userId;
			StackExperiences = stackExperiences;
			JobExperiences = jobExperiences;
			Certifications = certifications;
			Idioms = idioms;
			Projects = projects;
			Skills = skills;
			Hobies = hobies;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<Resume, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<Resume, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Title + item.Photo + item.ProfileValue).ToLower().Contains(searchTerm.ToLower()) ;
    private static Expression<Func<Resume, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Name + item.Title + item.Photo + item.ProfileValue).Contains(searchTerm) ;

    private static Func<IQueryable<Resume>, IOrderedQueryable<Resume>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderBy(item => item.Name),
			"Title" => items.OrderBy(item => item.Title),
			"Photo" => items.OrderBy(item => item.Photo),
			"DateOfBirth" => items.OrderBy(item => item.DateOfBirth),
			"ProfileValue" => items.OrderBy(item => item.ProfileValue)
        };
    private static Func<IQueryable<Resume>, IOrderedQueryable<Resume>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Name" => items.OrderByDescending(item => item.Name),
			"Title" => items.OrderByDescending(item => item.Title),
			"Photo" => items.OrderByDescending(item => item.Photo),
			"DateOfBirth" => items.OrderByDescending(item => item.DateOfBirth),
			"ProfileValue" => items.OrderByDescending(item => item.ProfileValue)
        };

    public static bool FilterFunc(Resume item, string searchTerm) 
        => (item.Name + item.Title + item.Photo + item.ProfileValue).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<Resume>, IOrderedQueryable<Resume>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<Resume, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<Resume>, IOrderedQueryable<Resume>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
