using System.ComponentModel.DataAnnotations;

namespace Recruiva.Web.Entities;

public class Candidate : BaseEntity
{
    public Address Address { get; set; }

    public string AddressId { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public DateTime DateOfBirth { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool IsEmailVerified { get; set; } = false;

    public bool IsPhoneVerified { get; set; } = false;

    public string? LinkedIn { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public EAccountStatus Status { get; set; } = EAccountStatus.Incomplete;
}