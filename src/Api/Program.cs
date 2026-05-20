using MenuNews.Api.GrpcServices;
using MenuNews.Api.Middleware;
using MenuNews.Application;
using MenuNews.Infrastructure;
using Grpc.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.MapGrpcService<NewsGrpcService>();

if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

app.Run();
