﻿using Recruiva.Core.Entities.Base;
using Recruiva.Core.Enums;
using Recruiva.Core.ValueObjects;

using System.ComponentModel.DataAnnotations;

namespace Recruiva.Core.Entities;

public class Candidate : BaseEntity
{
    public Address Address { get; set; }

    public Id AddressId { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = [];

    public DateTime DateOfBirth { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool IsEmailVerified { get; set; } = false;

    public bool IsPhoneVerified { get; set; } = false;

    public string? LinkedIn { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public virtual ICollection<Resume> Resumes { get; set; } = [];

    public EAccountStatus Status { get; set; } = EAccountStatus.Incomplete;
}