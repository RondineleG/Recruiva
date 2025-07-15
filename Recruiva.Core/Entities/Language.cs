using Recruiva.Web.Enums;

namespace Recruiva.Web.Entities;

public class Language
{
    public ELanguageLevel Level { get; set; }

    public string Name { get; set; } = string.Empty;
}