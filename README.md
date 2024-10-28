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
Unhealthy Response
```
{
    "status": "Unhealthy",
    "totalDuration": "00:00:10.2503276",
    "entries": {
        "npgsql": {
            "data": {},
            "duration": "00:00:00.0228878",
            "status": "Healthy",
            "tags": []
        },
        "redis": {
            "data": {},
            "description": "Timeout awaiting response (outbound=0KiB, inbound=0KiB, 5156ms elapsed, timeout is 5000ms), command=PING, next: PING, inst: 0, qu: 0, qs: 1, aw: False, bw: Inactive, rs: ReadAsync, ws: Idle, in: 0, in-pipe: 0, out-pipe: 0, last-in: 349, cur-in: 0, sync-ops: 0, async-ops: 3, serverEndpoint: localhost:6379, conn-sec: 600.07, aoc: 1, mc: 1/1/0, mgr: 10 of 10 available, clientName: DESKTOP-ELRSEMI(SE.Redis-v2.7.27.49176), IOCP: (Busy=0,Free=1000,Min=1,Max=1000), WORKER: (Busy=1,Free=32766,Min=20,Max=32767), POOL: (Threads=4,QueuedItems=0,CompletedItems=831,Timers=2), v: 2.7.27.49176 (Please take a look at this article for some common client-side issues that can cause timeouts: https://stackexchange.github.io/StackExchange.Redis/Timeouts)",
            "duration": "00:00:10.1897404",
            "exception": "Timeout awaiting response (outbound=0KiB, inbound=0KiB, 5156ms elapsed, timeout is 5000ms), command=PING, next: PING, inst: 0, qu: 0, qs: 1, aw: False, bw: Inactive, rs: ReadAsync, ws: Idle, in: 0, in-pipe: 0, out-pipe: 0, last-in: 349, cur-in: 0, sync-ops: 0, async-ops: 3, serverEndpoint: localhost:6379, conn-sec: 600.07, aoc: 1, mc: 1/1/0, mgr: 10 of 10 available, clientName: DESKTOP-ELRSEMI(SE.Redis-v2.7.27.49176), IOCP: (Busy=0,Free=1000,Min=1,Max=1000), WORKER: (Busy=1,Free=32766,Min=20,Max=32767), POOL: (Threads=4,QueuedItems=0,CompletedItems=831,Timers=2), v: 2.7.27.49176 (Please take a look at this article for some common client-side issues that can cause timeouts: https://stackexchange.github.io/StackExchange.Redis/Timeouts)",
            "status": "Unhealthy",
            "tags": []
        }
    }
}
```
