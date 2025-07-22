using System.ComponentModel.DataAnnotations;

namespace Recruiva.Web.Entities;

public class Advertiser : BaseEntity
{
    public string? ActivePlan { get; set; }

    public Address? Address { get; set; }

    public Id AddressId { get; set; }

    public string? CompanyDescription { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool IsEmailVerified { get; set; } = false;

    public bool IsPhoneVerified { get; set; } = false;

    public virtual ICollection<Job> Jobs { get; set; } = [];

    public string? LogoUrl { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public EPersonType PersonType { get; set; }

    [Required]
    public string Phone { get; set; } = string.Empty;

    public EAdvertiserStatus Status { get; set; } = EAdvertiserStatus.Incomplete;

    [Required]
    public string TaxId { get; set; } = string.Empty;

    public string? Website { get; set; }
}