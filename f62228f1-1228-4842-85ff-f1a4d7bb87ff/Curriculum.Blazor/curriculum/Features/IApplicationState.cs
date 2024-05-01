using Mud = MudBlazor;
using Curriculum.EF.Models;

namespace Curriculum.Blazor;

public interface IApplicationState
{
    Address ActiveAddress {get;}
	Certification ActiveCertification {get;}
	Contact ActiveContact {get;}
	Group ActiveGroup {get;}
	Idiom ActiveIdiom {get;}
	JobExperience ActiveJobExperience {get;}
	Permission ActivePermission {get;}
	Project ActiveProject {get;}
	Resume ActiveResume {get;}
	ScreenShot ActiveScreenShot {get;}
	Skill ActiveSkill {get;}
	StackExperience ActiveStackExperience {get;}
	User ActiveUser {get;}

    Mud.MudTheme ActiveTheme { get; }
    string ActiveThemeName { get; }

    string ActiveType { get; }
    void SetActiveItem(string type, object item);

    bool IsToolBoxOpen { get; set; }
    void ToggleToolBox();

    bool IsLoading { get; set; }
    void ToggleLoading();

    UserDTO CurrentUser { get; }
    void SetCurrentUser(UserDTO user);

    string CurrentRoute { get; }
    void SetCurrentRoute(string route);

    List<Mud.BreadcrumbItem> BreadcrumItems { get; }
    void PushBreadcrumItem(bool isBaseClass = false, params Mud.BreadcrumbItem[] items);
    Mud.BreadcrumbItem PopBreadcrumItem();

    event Action OnChange;
}
