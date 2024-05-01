using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class UpdateContactRequest
{
    [JsonProperty("email")]
	public string? Email { get;  set; } = "Default";

	[JsonProperty("phone")]
	public string? Phone { get;  set; } = "Default";

	[JsonProperty("mobile")]
	public string? Mobile { get;  set; } = "Default";

	[JsonProperty("isDeleted")]
	public bool IsDeleted { get;  set; } = false;
    [JsonProperty("userId")]
    public Guid UserId {get; set;}

}