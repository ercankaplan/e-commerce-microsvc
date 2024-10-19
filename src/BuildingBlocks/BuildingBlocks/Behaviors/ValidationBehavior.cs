using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validator)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        //public ValidationBehavior(IEnumerable<IValidator<TRequest>> validator) { }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var valContext = new ValidationContext<TRequest>(request);

            var valResult = await Task.WhenAll(validator.Select(x => x.ValidateAsync(valContext, cancellationToken)));

            var failures = valResult.Where(x => !x.IsValid).SelectMany(s => s.Errors).ToList();

            if (failures.Count > 0)
                throw new ValidationException(failures);

            return await next();
        }
    }
}
