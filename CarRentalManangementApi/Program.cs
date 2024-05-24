using DevExpress.ExpressApp.MiddleTier;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
//builder.Services.AddSingleton<UnitOfWork>(); // Register UnitOfWork as a singleton

// Load connection string from configuration
//string connectionString1 = ("XpoProvider=MySql;server=Localhost;user id=root; password=admin; database=cms;persist security info=true;CharSet=utf8;Port=3306;");



// Configure XPO
//builder.Services.AddSingleton((serviceProvider) =>
//{
//    XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
//    XpoDefault.Session = null; // Use a separate session for each request
//    return XpoDefault.DataLayer;
//});

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