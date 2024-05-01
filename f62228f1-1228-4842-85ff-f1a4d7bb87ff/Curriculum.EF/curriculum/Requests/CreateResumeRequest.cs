using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class CreateResumeRequest
{
    [JsonProperty("name")]
	public string Name { get;  set; } = "Default";

	[JsonProperty("title")]
	public string Title { get;  set; } = "Default";

	[JsonProperty("photo")]
	public string Photo { get;  set; } = "Default";

	[JsonProperty("status")]
	public eStatus Status { get;  set; } = eStatus.Default;

	[JsonProperty("dateOfBirth")]
	public DateTime DateOfBirth { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("profileValue")]
	public string ProfileValue { get;  set; } = "Default";

	[JsonProperty("emails")]
	public string[] Emails { get;  set; } = new string[] {};

	[JsonProperty("phones")]
	public string[] Phones { get;  set; } = new string[] {};

	[JsonProperty("userId")]
	public Guid UserId { get;  set; } = Guid.NewGuid();

	[JsonProperty("stackExperiences")]
	public ICollection<StackExperience> StackExperiences { get;  set; } = new List<StackExperience>();

	[JsonProperty("jobExperiences")]
	public ICollection<JobExperience> JobExperiences { get;  set; } = new List<JobExperience>();

	[JsonProperty("certifications")]
	public ICollection<Certification> Certifications { get;  set; } = new List<Certification>();

	[JsonProperty("idioms")]
	public ICollection<Idiom> Idioms { get;  set; } = new List<Idiom>();

	[JsonProperty("projects")]
	public ICollection<Project> Projects { get;  set; } = new List<Project>();

	[JsonProperty("skills")]
	public ICollection<Skill> Skills { get;  set; } = new List<Skill>();

	[JsonProperty("hobies")]
	public string[] Hobies { get;  set; } = new string[] {};
}