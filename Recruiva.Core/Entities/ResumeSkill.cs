namespace Recruiva.Web.Entities;

public class ResumeSkill
{
    public string Level { get; set; } = string.Empty;

    public virtual Resume? Resume { get; set; }

    public string ResumeId { get; set; } = string.Empty;

    public string Skill { get; set; } = string.Empty;

    public float YearsOfExperience { get; set; }
}