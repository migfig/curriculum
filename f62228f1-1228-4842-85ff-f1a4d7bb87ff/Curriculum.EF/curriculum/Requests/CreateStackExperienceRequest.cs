using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class CreateStackExperienceRequest
{
    [JsonProperty("description")]
	public string Description { get;  set; } = "Default";

    [JsonProperty("ordinal")]
	public int Ordinal { get;  set; } = 0;

	[JsonProperty("tags")]
	public string[] Tags { get;  set; } = new string[] {};
    [JsonProperty("resumeId")]
    public Guid ResumeId {get; set;}

}