using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class CreateJobExperienceRequest
{
    [JsonProperty("company")]
	public string Company { get;  set; } = "Default";

	[JsonProperty("location")]
	public string Location { get;  set; } = "Default";

	[JsonProperty("startDate")]
	public DateTime StartDate { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("endDate")]
	public DateTime EndDate { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("position")]
	public string Position { get;  set; } = "Default";

	[JsonProperty("jobDescription")]
	public string JobDescription { get;  set; } = "Default";

	[JsonProperty("achievements")]
	public string[] Achievements { get;  set; } = new string[] {};

	[JsonProperty("functionalExperiences")]
	public string[] FunctionalExperiences { get;  set; } = new string[] {};

	[JsonProperty("usedTools")]
	public string[] UsedTools { get;  set; } = new string[] {};
 
  	[JsonProperty("frameworks")]
 	public string[] Frameworks { get;  set; } = new string[] {};

    [JsonProperty("resumeId")]
    public Guid ResumeId {get; set;}

}