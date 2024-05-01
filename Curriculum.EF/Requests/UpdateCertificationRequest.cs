using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class UpdateCertificationRequest
{
    [JsonProperty("title")]
	public string Title { get;  set; } = "Default";

	[JsonProperty("schoolName")]
	public string SchoolName { get;  set; } = "Default";

	[JsonProperty("location")]
	public string Location { get;  set; } = "Default";

	[JsonProperty("isEducation")]
	public bool IsEducation { get;  set; } = false;

	[JsonProperty("startDate")]
	public DateTime StartDate { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("endDate")]
	public DateTime EndDate { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("expireDate")]
	public DateTime ExpireDate { get;  set; } = DateTime.Now.ToUniversalTime();
    [JsonProperty("resumeId")]
    public Guid ResumeId {get; set;}

}