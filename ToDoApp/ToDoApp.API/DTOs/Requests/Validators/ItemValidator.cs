using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;
using ToDoApp.Data.Utils.Enums;

namespace ToDoApp.API.DTOs.Requests.Validators;

public class ItemValidator: AbstractValidator<ItemDTO>
{
    #region [- Constructor -]
    public ItemValidator()
    {
        RuleFor(p => p.Title).NotNull().WithMessage("Enter a title for item")
                             .NotEmpty().WithMessage("Enter a title for item")
                             .MaximumLength(100).WithMessage("Your characters length for title is more than valid limit")
                             .Must(ValidateTitle).WithMessage("Title should contains just the lower and upper case characters and space");

        RuleFor(p => p.Description).MaximumLength(500).WithMessage("Your characters length for description is more than valid limit");

        RuleFor(p => p.Priority).NotNull().WithMessage("Enter priority of item");
                                                          
    }
    #endregion

    #region [- Methods -]

    #region [- ValidateTitle -]
    private bool ValidateTitle(string? title)
    {
        // In Current db design sql type of title is varchar(100) for english support
        // But If sql type of title changed to nvarchar(100) for persian support
        // The regex pattern for persian alphabets will be this below pattern 
        // @"^[\u0622\u0627\u0628\u067E\u062A-\u062C\u0686\u062D-\u0632\u0698\u0633-\u063A\u0641\u0642\u06A9\u06AF\u0644-\u0648\u06CC\s]+$"

        bool isValidPattern = true;
        if (!string.IsNullOrEmpty(title))
            if (title.Length <= 100)
                isValidPattern = Regex.IsMatch(title, @"^[a-zA-Z\s]+$");
         
        return isValidPattern;
    }
    #endregion

    #endregion

}
