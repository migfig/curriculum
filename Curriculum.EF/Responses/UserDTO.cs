using System;
using System.Collections.Generic;

namespace Curriculum.EF.Models;

public class UserDTO
{
    public Guid Id { get; protected set; }
    public string? Email { get; protected set; }
    public string? Name { get; protected set; }
    public string? Phone { get; protected set; }
    public string? JobTitle { get; protected set; }
    public string? FirstName { get; protected set; }
    public string? LastName { get; protected set; }
    public string? Avatar { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTime? DateCreated { get; protected set; }
    public Guid? CreatedBy { get; protected set; }
    public DateTime? DateUpdated { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }
    public string? Token { get; set; }

    public ICollection<Address> Addresses { get; protected set; } = new List<Address>();
    public ICollection<Contact> Contacts { get; protected set; } = new List<Contact>();
    public ICollection<Group> Groups { get; protected set; } = new List<Group>();

    public int Version { get; protected set; }

    static public UserDTO FromUserBase(User user, string token)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Phone = user.Phone,
            JobTitle = user.JobTitle,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Avatar = user.Avatar,
            IsDeleted = user.IsDeleted,
            DateCreated = user.DateCreated,
            CreatedBy = user.CreatedBy,
            DateUpdated = user.DateUpdated,
            UpdatedBy = user.UpdatedBy,
            Token = token,
            Addresses = user.Addresses,
            Contacts = user.Contacts,
            Groups = user.Groups
        };
    }
}