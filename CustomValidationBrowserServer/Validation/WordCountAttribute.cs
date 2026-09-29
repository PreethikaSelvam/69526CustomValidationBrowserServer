using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Components.Forms;

namespace CustomValidationBrowserServer.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WordCountAttribute(int minimum, int maximum)
    : ValidationAttribute, IClientValidationRuleProvider
{
    public int Minimum { get; } = minimum;

    public int Maximum { get; } = maximum;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string text || string.IsNullOrWhiteSpace(text))
        {
            return ValidationResult.Success;
        }

        var wordCount = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        return wordCount >= Minimum && wordCount <= Maximum
            ? ValidationResult.Success
            : new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                [validationContext.MemberName!]);
    }

    public override string FormatMessage(string messageTemplate, string displayName)
    {
        return string.Format(
            CultureInfo.CurrentCulture,
            messageTemplate,
            displayName,
            Minimum,
            Maximum);
    }

    public override string FormatErrorMessage(string name)
    {
        return FormatMessage(
            "{0} must contain at least {1} and at most {2} words.",
            name);
    }

    public IEnumerable<ClientValidationRule> GetClientValidationRules()
    {
        yield return new ClientValidationRule(
            "wordcount",
            new Dictionary<string, string>
            {
                ["minimum"] = Minimum.ToString(CultureInfo.InvariantCulture),
                ["maximum"] = Maximum.ToString(CultureInfo.InvariantCulture)
            });
    }
}
