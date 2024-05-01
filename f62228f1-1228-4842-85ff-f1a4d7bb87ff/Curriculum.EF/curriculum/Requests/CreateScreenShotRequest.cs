using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class CreateScreenShotRequest
{
    [JsonProperty("name")]
	public string Name { get;  set; } = "Default";

	[JsonProperty("url")]
	public string Url { get;  set; } = "Default";
    [JsonProperty("projectId")]
    public Guid ProjectId {get; set;}

    [JsonProperty("resumeId")]
    public Guid ResumeId {get; set;}
}