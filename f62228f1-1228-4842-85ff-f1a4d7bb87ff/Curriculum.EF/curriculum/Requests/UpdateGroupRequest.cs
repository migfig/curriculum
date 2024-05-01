using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class UpdateGroupRequest
{
    [JsonProperty("name")]
	public string? Name { get;  set; } = "Default";

	[JsonProperty("permissions")]
	public ICollection<Permission> Permissions { get;  set; } = new List<Permission>();
    [JsonProperty("userId")]
    public Guid UserId {get; set;}

}