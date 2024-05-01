using Mud = MudBlazor;
using Curriculum.EF.Models;

namespace Curriculum.Blazor;

public class ApplicationState : IApplicationState
{
    private Address _activeAddress = new Address { };
    private Certification _activeCertification = new Certification { };
    private Contact _activeContact = new Contact { };
    private Group _activeGroup = new Group { };
    private Idiom _activeIdiom = new Idiom { };
    private JobExperience _activeJobExperience = new JobExperience { };
    private Permission _activePermission = new Permission { };
    private Project _activeProject = new Project { };
    private Resume _activeResume = new Resume { };
    private ScreenShot _activeScreenShot = new ScreenShot { };
    private Skill _activeSkill = new Skill { };
    private StackExperience _activeStackExperience = new StackExperience { };
    private User _activeUser = new User { };
    public Address ActiveAddress => _activeAddress;
    public Certification ActiveCertification => _activeCertification;
    public Contact ActiveContact => _activeContact;
    public Group ActiveGroup => _activeGroup;
    public Idiom ActiveIdiom => _activeIdiom;
    public JobExperience ActiveJobExperience => _activeJobExperience;
    public Permission ActivePermission => _activePermission;
    public Project ActiveProject => _activeProject;
    public Resume ActiveResume => _activeResume;
    public ScreenShot ActiveScreenShot => _activeScreenShot;
    public Skill ActiveSkill => _activeSkill;
    public StackExperience ActiveStackExperience => _activeStackExperience;
    public User ActiveUser => _activeUser;
    private Mud.MudTheme _activeTheme = new Mud.MudTheme { };
    private string _activeThemeName = "Blue-Spare";

    public Mud.MudTheme ActiveTheme => _activeTheme;
    public string ActiveThemeName => _activeThemeName;

    private string _activeType = string.Empty;
    public string ActiveType => _activeType;
    public void SetActiveItem(string type, object item)
    {
        _activeType = type;
        switch (type)
        {
            case "Address": _activeAddress = item as Address; break;
            case "Certification": _activeCertification = item as Certification; break;
            case "Contact": _activeContact = item as Contact; break;
            case "Group": _activeGroup = item as Group; break;
            case "Idiom": _activeIdiom = item as Idiom; break;
            case "JobExperience": _activeJobExperience = item as JobExperience; break;
            case "Permission": _activePermission = item as Permission; break;
            case "Project": _activeProject = item as Project; break;
            case "Resume": _activeResume = item as Resume; break;
            case "ScreenShot": _activeScreenShot = item as ScreenShot; break;
            case "Skill": _activeSkill = item as Skill; break;
            case "StackExperience": _activeStackExperience = item as StackExperience; break;
            case "User": _activeUser = item as User; break;
            case "MudTheme": _activeTheme = item as Mud.MudTheme; break;
            case "ThemeName": _activeThemeName = item as string; break;
        }
        NotifyStateChanged();
    }

    public bool IsToolBoxOpen { get; set; } = false;
    public void ToggleToolBox()
    {
        IsToolBoxOpen = !IsToolBoxOpen;
        NotifyStateChanged();
    }

    public bool IsLoading { get; set; } = false;
    public void ToggleLoading()
    {
        IsLoading = !IsLoading;
        NotifyStateChanged();
    }

    private UserDTO _currentUser = new UserDTO { };
    public UserDTO CurrentUser => _currentUser;
    public void SetCurrentUser(UserDTO user)
    {
        _currentUser = user;
        NotifyStateChanged();
    }

    private string _currentRoute = string.Empty;
    public string CurrentRoute => _currentRoute;
    public void SetCurrentRoute(string route)
    {
        _currentRoute = route;
        NotifyStateChanged();
    }

    private Stack<Mud.BreadcrumbItem> _breadcrumItems = new Stack<Mud.BreadcrumbItem>();
    public List<Mud.BreadcrumbItem> BreadcrumItems => _breadcrumItems.ToList();
    public void PushBreadcrumItem(bool isBaseClass = false, params Mud.BreadcrumbItem[] items)
    {
        if (isBaseClass) _breadcrumItems.Clear();
        foreach (var item in items)
        {
            _breadcrumItems.Push(item);
        }
        NotifyStateChanged();
    }

    public Mud.BreadcrumbItem PopBreadcrumItem()
    {
        var item = _breadcrumItems.Pop();
        NotifyStateChanged();
        return item;
    }

    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
}
