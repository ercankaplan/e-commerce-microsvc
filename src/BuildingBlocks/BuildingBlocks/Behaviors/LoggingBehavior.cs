using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull , IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request = {Request} - response = {Response} - RequesstData={RequestData}", typeof(TRequest).Name, typeof(TResponse).Name, request);


            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();

            if (timer.Elapsed.Seconds > 3)
                logger.LogWarning("[PERFORMANCE] The Request {Request} took {TimeTaken} sec.", typeof(TRequest).Name, timer.Elapsed.Seconds);

            logger.LogInformation("[END] Handled {Request} with  {Response}", typeof(TRequest).Name, typeof(TResponse).Name);

            return response;
        }
    }
}
