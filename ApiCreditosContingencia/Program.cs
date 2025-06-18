using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Interface;
using DataLayer.RN;
using BLL.Interface;
using BLL.RN;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("stopConnection")));


builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ICreditosService, CreditoService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Créditos Contingencia",
        Version = "v1",
        Description = "API para gestionar créditos de clientes"
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

if (app.Environment.IsDevelopment())
{

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Créditos Contingencia v1");
    });
}
else
{

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Créditos Contingencia v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
