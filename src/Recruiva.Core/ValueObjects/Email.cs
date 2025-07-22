using Recruiva.Core.Exceptions;
using Recruiva.Core.Validations;
using Recruiva.Core.ValueObjects.Base;
using Recruiva.Web.Resources.Core.ValueObjects;

using System.Globalization;
using System.Text.RegularExpressions;


namespace Recruiva.Core.ValueObjects;

/// <summary>Documentation for Email class.</summary>
public partial class Email : ValueObject
{
    private Email(string value, ValidationResult validationResult)
    {
        Value = value;
        Validate();
        _validationResult = validationResult;
    }

    private readonly ValidationResult _validationResult;

    public string Value { get; }

    public static Email Create(string value)
    {
        DomainException.ThrowErrorWhen(() => string.IsNullOrWhiteSpace(value), EmailResources.EmailCannotBeEmpty);
        var email = new Email(value, new ValidationResult());
        email.Validate();
        return email;
    }

    public override ValidationResult Validate()
    {
        var regex = MyRegex();
        DomainException.ThrowErrorWhen(() => !regex.IsMatch(Value), EmailResources.InvalidEmail);

        return _validationResult;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower(CultureInfo.CurrentCulture);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex MyRegex();
}