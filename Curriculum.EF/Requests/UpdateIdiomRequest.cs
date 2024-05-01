using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class UpdateIdiomRequest
{
    [JsonProperty("name")]
	public string Name { get;  set; } = "Default";

	[JsonProperty("level")]
	public string Level { get;  set; } = "Default";

	[JsonProperty("schoolName")]
	public string SchoolName { get;  set; } = "Default";

	[JsonProperty("location")]
	public string Location { get;  set; } = "Default";
    [JsonProperty("resumeId")]
    public Guid ResumeId {get; set;}

}