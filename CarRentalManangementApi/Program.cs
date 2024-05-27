using DevExpress.ExpressApp.MiddleTier;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//json options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor service
builder.Services.ConfigureOptions<ConfigureJsonOptions>();
//database connections
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
builder.Services.AddScoped<UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
public class ConfigureJsonOptions : IConfigureOptions<JsonOptions>, IServiceProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;

    public ConfigureJsonOptions(
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
    }

    public void Configure(JsonOptions options)
    {
        options.JsonSerializerOptions.Converters.Add(new PersistentBaseConverterFactory(this));
    }

    public object GetService(Type serviceType)
    {
        return (_httpContextAccessor.HttpContext?.RequestServices ?? _serviceProvider).GetService(serviceType);
    }
}