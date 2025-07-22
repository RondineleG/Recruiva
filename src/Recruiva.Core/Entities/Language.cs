using Recruiva.Core.Enums;

namespace Recruiva.Core.Entities;

public class Language
{
    public ELanguageLevel Level { get; set; }

    public string Name { get; set; } = string.Empty;
}