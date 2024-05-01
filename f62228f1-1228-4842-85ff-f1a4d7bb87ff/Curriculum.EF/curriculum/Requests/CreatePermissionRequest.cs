using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class CreatePermissionRequest
{
    [JsonProperty("name")]
	public string? Name { get;  set; } = "Default";
    [JsonProperty("groupId")]
    public Guid GroupId {get; set;}

    [JsonProperty("userId")]
    public Guid UserId {get; set;}
}