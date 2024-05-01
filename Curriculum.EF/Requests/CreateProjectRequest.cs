using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class CreateProjectRequest
{
    [JsonProperty("description")]
	public string Description { get;  set; } = "Default";

	[JsonProperty("isStack")]
	public bool IsStack { get;  set; } = false;

	[JsonProperty("startDate")]
	public DateTime StartDate { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("endDate")]
	public DateTime EndDate { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("usedTools")]
	public string[] UsedTools { get;  set; } = new string[] {};

	[JsonProperty("screenShots")]
	public ICollection<ScreenShot> ScreenShots { get;  set; } = new List<ScreenShot>();
    [JsonProperty("resumeId")]
    public Guid ResumeId {get; set;}

}