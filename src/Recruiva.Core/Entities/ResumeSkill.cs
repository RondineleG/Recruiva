namespace Recruiva.Web.Entities;

public class ResumeSkill : BaseEntity
{
    public string Level { get; set; } = string.Empty;

    public virtual Resume? Resume { get; set; }

    public Id ResumeId { get; set; }

    public string Skill { get; set; } = string.Empty;

    public float YearsOfExperience { get; set; }
}