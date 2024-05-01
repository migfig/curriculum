using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class UpdateAddressRequest
{
    [JsonProperty("street")]
	public string? Street { get;  set; } = "Default";

	[JsonProperty("exteriorNumber")]
	public string? ExteriorNumber { get;  set; } = "Default";

	[JsonProperty("interiorNumber")]
	public string? InteriorNumber { get;  set; } = "Default";

	[JsonProperty("postalCode")]
	public string? PostalCode { get;  set; } = "Default";

	[JsonProperty("city")]
	public string? City { get;  set; } = "Default";

	[JsonProperty("state")]
	public string? State { get;  set; } = "Default";

	[JsonProperty("country")]
	public string? Country { get;  set; } = "Default";

	[JsonProperty("isDeleted")]
	public bool IsDeleted { get;  set; } = false;

	[JsonProperty("zip")]
	public string Zip { get;  set; } = "Default";
    [JsonProperty("userId")]
    public Guid UserId {get; set;}

}