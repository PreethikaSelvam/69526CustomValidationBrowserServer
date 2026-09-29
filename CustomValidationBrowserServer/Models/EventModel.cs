using System.ComponentModel.DataAnnotations;
using CustomValidationBrowserServer.Validation;
using Microsoft.Extensions.Validation;

namespace CustomValidationBrowserServer.Models;

[ValidatableType]
public sealed class EventModel
{
    [Display(Name = "EventModel_Title_DisplayName")]
    [WordCount(3, 6)]
    public string? Title { get; set; }
}
