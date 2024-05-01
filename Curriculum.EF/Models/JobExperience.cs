using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curriculum.EF.Models;

public class JobExperience
{
    [Required]
	public Guid Id { get;  set; } = Guid.NewGuid();
	public string Company { get;  set; } = "Default";
	public string Location { get;  set; } = "Default";
	public DateTime StartDate { get;  set; } = DateTime.Now.ToUniversalTime();
	public DateTime EndDate { get;  set; } = DateTime.Now.ToUniversalTime();
	public string Position { get;  set; } = "Default";
	[MaxLength(256)]
	public string JobDescription { get;  set; } = "Default";
	public string[] Achievements { get;  set; } = new string[] {};
	public string[] FunctionalExperiences { get;  set; } = new string[] {};
	public string[] UsedTools { get;  set; } = new string[] {};
	public string[] Frameworks { get;  set; } = new string[] {};

	[Required]
	public Guid ResumeId { get;  set; } = Guid.NewGuid();
	public Resume? Resume { get;  set; }

    // For serialization
    public JobExperience() { }

    public static JobExperience CreateJobExperience(
        Guid id,
		string company,
		string location,
		DateTime startDate,
		DateTime endDate,
		string position,
		string jobDescription,
		string[] achievements,
		string[] functionalExperiences,
		string[] usedTools,
		string[] frameworks,
		Guid resumeId
    )
    {
        return new JobExperience(
            id,
			company,
			location,
			startDate,
			endDate,
			position,
			jobDescription,
			achievements,
			functionalExperiences,
			usedTools,
			frameworks,
			resumeId
        );
    }

    private JobExperience(
        Guid id,
		string company,
		string location,
		DateTime startDate,
		DateTime endDate,
		string position,
		string jobDescription,
		string[] achievements,
		string[] functionalExperiences,
		string[] usedTools,
		string[] frameworks,
		Guid resumeId
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));


        Id = id;
			Company = company;
			Location = location;
			StartDate = startDate;
			EndDate = endDate;
			Position = position;
			JobDescription = jobDescription;
			Achievements = achievements;
			FunctionalExperiences = functionalExperiences;
			UsedTools = usedTools;
			Frameworks = frameworks;
			ResumeId = resumeId;
    }

    public void Update(
        string company,
		string location,
		DateTime startDate,
		DateTime endDate,
		string position,
		string jobDescription,
		string[] achievements,
		string[] functionalExperiences,
		string[] usedTools,
		string[] frameworks,
		Guid resumeId
    )
    {


        Company = company;
			Location = location;
			StartDate = startDate;
			EndDate = endDate;
			Position = position;
			JobDescription = jobDescription;
			Achievements = achievements;
			FunctionalExperiences = functionalExperiences;
			UsedTools = usedTools;
			Frameworks = frameworks;
			ResumeId = resumeId;
    }

    public void Delete()
    {

    }

    #region expressions
    private static Expression<Func<JobExperience, bool>> GuidFilter(Guid id) 
        => (item) => item.Id == id;        
    private static Expression<Func<JobExperience, bool>> DefaultLowerCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Company + item.Location + item.Position + item.JobDescription).ToLower().Contains(searchTerm.ToLower()) && (parentId != null ? item.ResumeId == parentId : true);
    private static Expression<Func<JobExperience, bool>> DefaultIgnoreCaseFilter(string searchTerm, Guid? parentId) 
        => (item) => (item.Company + item.Location + item.Position + item.JobDescription).Contains(searchTerm) && (parentId != null ? item.ResumeId == parentId : true);

    private static Func<IQueryable<JobExperience>, IOrderedQueryable<JobExperience>> DefaultOrderByAscending(string orderBy)
        => (items) => orderBy switch {
			"Company" => items.OrderBy(item => item.Company),
			"Location" => items.OrderBy(item => item.Location),
			"StartDate" => items.OrderBy(item => item.StartDate),
			"EndDate" => items.OrderBy(item => item.EndDate),
			"Position" => items.OrderBy(item => item.Position),
			"JobDescription" => items.OrderBy(item => item.JobDescription)
        };
    private static Func<IQueryable<JobExperience>, IOrderedQueryable<JobExperience>> DefaultOrderByDescending(string orderBy)
        => (items) => orderBy switch {
			"Company" => items.OrderByDescending(item => item.Company),
			"Location" => items.OrderByDescending(item => item.Location),
			"StartDate" => items.OrderByDescending(item => item.StartDate),
			"EndDate" => items.OrderByDescending(item => item.EndDate),
			"Position" => items.OrderByDescending(item => item.Position),
			"JobDescription" => items.OrderByDescending(item => item.JobDescription)
        };

    public static bool FilterFunc(JobExperience item, string searchTerm) 
        => (item.Company + item.Location + item.Position + item.JobDescription).ToLower().Contains(searchTerm.ToLower());

    private static Func<IQueryable<JobExperience>, IOrderedQueryable<JobExperience>> DefaultOrderByIgnore(string orderBy)
        => null;

    public static Expression<Func<JobExperience, bool>> GetFilterExpr(string searchTerm, eSearchCase searchCase, Guid? parentId = null) {
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

    public static Func<IQueryable<JobExperience>, IOrderedQueryable<JobExperience>> GetSortFunc(string sortBy, eSortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) return null; 

        return sortDirection switch {
            eSortDirection.Ascending => DefaultOrderByAscending(sortBy),
            eSortDirection.Descending => DefaultOrderByDescending(sortBy),
            eSortDirection.None => DefaultOrderByIgnore(sortBy)
        };
    }

    #endregion expressions
}
