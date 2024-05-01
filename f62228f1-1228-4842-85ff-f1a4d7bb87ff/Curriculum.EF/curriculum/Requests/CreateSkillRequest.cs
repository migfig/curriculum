using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class CreateSkillRequest
{
    [JsonProperty("name")]
	public string Name { get;  set; } = "Default";

	[JsonProperty("stack")]
	public string Stack { get;  set; } = "Default";

	[JsonProperty("level")]
	public string Level { get;  set; } = "Default";

	[JsonProperty("percentage")]
	public double Percentage { get;  set; } = 0.0;

	[JsonProperty("ordinal")]
	public int Ordinal { get;  set; } = 0;
	
	[JsonProperty("isSoft")]
	public bool IsSoft { get;  set; } = false;

    [JsonProperty("resumeId")]
    public Guid ResumeId {get; set;}

}