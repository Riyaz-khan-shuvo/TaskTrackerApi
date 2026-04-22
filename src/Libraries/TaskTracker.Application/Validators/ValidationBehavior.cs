using FluentValidation;
using MediatR;

namespace TaskTracker.Application.Validators
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    var resultType = typeof(TResponse);

                    if (resultType == typeof(TaskTracker.Application.DTOs.Common.ResultVM))
                    {
                        var messages = string.Join(" | ", failures.Select(f => f.ErrorMessage));
                        return (TResponse)(object)new TaskTracker.Application.DTOs.Common.ResultVM
                        {
                            Status = "Fail",
                            Message = messages
                        };
                    }

                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }
}
