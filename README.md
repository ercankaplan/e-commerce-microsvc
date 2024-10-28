# e-commerce-microsvc
e-commerce-microservices

This project implements CQRS by using the MediatR library, where commands and queries are handled by separate handlers. 
Mapster would be used to map between entities and DTOs or other models.
The BuildingBlocks project provides abstractions for the CQRS pattern that are utilized in services like Catalog.API.

How to use PostgreSQL image
https://hub.docker.com/_/postgres

A distributed cache can improve the performance and scalability of an ASP.NET Core app, especially when the app is hosted by a cloud service
https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0

Redis Image
https://hub.docker.com/_/redis

Health Check
https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
https://www.nuget.org/packages/AspNetCore.HealthChecks.NpgSql
https://www.nuget.org/packages/AspNetCore.HealthChecks.Redis

``` HealtCheck Response
{
    "status": "Healthy",
    "totalDuration": "00:00:00.3067133",
    "entries": {
        "npgsql": {
            "data": {},
            "duration": "00:00:00.3047232",
            "status": "Healthy",
            "tags": []
        },
        "redis": {
            "data": {},
            "duration": "00:00:00.2136677",
            "status": "Healthy",
            "tags": []
        }
    }
}
```
