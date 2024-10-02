var builder = WebApplication.CreateBuilder(args);

//Add services DI

var app = builder.Build();

//configures the http request pipeline.


app.Run();
