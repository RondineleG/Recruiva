using System.ComponentModel.DataAnnotations;

namespace Recruiva.Web.Entities;

public class Resume : BaseEntity
{
    public virtual Candidate? Candidate { get; set; }

    [Required]
    public string CandidateId { get; set; } = string.Empty;

    public virtual ICollection<Education> EducationHistory { get; set; } = new List<Education>();

    public virtual ICollection<Experience> ExperienceHistory { get; set; } = new List<Experience>();

    public bool IsActive { get; set; }

    public virtual ICollection<Language> Languages { get; set; } = new List<Language>();

    public virtual ICollection<ResumeSkill> Skills { get; set; } = new List<ResumeSkill>();

    public string? Summary { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}