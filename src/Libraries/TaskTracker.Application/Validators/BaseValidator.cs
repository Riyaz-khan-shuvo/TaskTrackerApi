using FluentValidation;
using System.Linq.Expressions;


namespace TaskTracker.Application.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        protected void ValidateEmail(System.Linq.Expressions.Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }

        protected void ValidatePassword(System.Linq.Expressions.Expression<Func<T, string>> expression, int minLength = 6)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(minLength).WithMessage($"Password must be at least {minLength} characters.");
        }

        protected void ValidateRequired(System.Linq.Expressions.Expression<Func<T, string>> expression, string fieldName)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage($"{fieldName} is required.");
        }

        protected void ValidateMaxLength(System.Linq.Expressions.Expression<Func<T, string>> expression, int maxLength, string fieldName)
        {
            RuleFor(expression)
                .MaximumLength(maxLength).WithMessage($"{fieldName} cannot exceed {maxLength} characters.");
        }

        protected void ValidateRegex(System.Linq.Expressions.Expression<Func<T, string>> expression, string pattern, string message)
        {
            RuleFor(expression)
                .Matches(pattern).WithMessage(message);
        }
        protected void ValidateName(Expression<Func<T, string>> expression, string? fieldName = null, int maxLength = 50)
        {
            var memberName = (expression.Body as MemberExpression)?.Member?.Name ?? "Name";
            var displayName = fieldName ?? memberName;

            RuleFor(expression)
                .NotEmpty().WithMessage($"{displayName} is required.")
                .MaximumLength(maxLength).WithMessage($"{displayName} cannot exceed {maxLength} characters.")
                .Matches("^[a-zA-ZÀ-ÿ'\\-\\s]+$").WithMessage($"{displayName} can only contain letters, spaces, hyphens, and apostrophes.");
        }


    }
}
