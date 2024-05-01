using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class UpdateUserRequest
{
    [JsonProperty("name")]
	public string? Name { get;  set; } = "Default";

	[JsonProperty("email")]
	public string? Email { get;  set; } = "Default";

	[JsonProperty("phone")]
	public string? Phone { get;  set; } = "Default";

	[JsonProperty("jobTitle")]
	public string? JobTitle { get;  set; } = "Default";

	[JsonProperty("firstName")]
	public string? FirstName { get;  set; } = "Default";

	[JsonProperty("lastName")]
	public string? LastName { get;  set; } = "Default";

	[JsonProperty("avatar")]
	public string? Avatar { get;  set; } = "Default";

	[JsonProperty("isDeleted")]
	public bool IsDeleted { get;  set; } = false;

	[JsonProperty("dateCreated")]
	public DateTime? DateCreated { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("createdBy")]
	public Guid? CreatedBy { get;  set; } = Guid.NewGuid();

	[JsonProperty("dateUpdated")]
	public DateTime? DateUpdated { get;  set; } = DateTime.Now.ToUniversalTime();

	[JsonProperty("updatedBy")]
	public Guid? UpdatedBy { get;  set; } = Guid.NewGuid();

	[JsonProperty("pwdHash")]
	public string? PwdHash { get;  set; } = "Default";

	[JsonProperty("pwdSalt")]
	public string? PwdSalt { get;  set; } = "Default";

	[JsonProperty("addresses")]
	public ICollection<Address> Addresses { get;  set; } = new List<Address>();

	[JsonProperty("contacts")]
	public ICollection<Contact> Contacts { get;  set; } = new List<Contact>();

	[JsonProperty("groups")]
	public ICollection<Group> Groups { get;  set; } = new List<Group>();
}