using Common.Shared.Filters;
using DataAccessLayer.Data;
using DataAccessLayer.Mappings;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using SupplierManagementAPI.Services;
using SupplierManagementAPI.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Load the shared appsettings
var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
var sharedConfigPath = Path.Combine(parentDirectory, "Common.Shared", "SharedConfigurations", "appsettings.shared.json");
builder.Configuration.AddJsonFile(sharedConfigPath, optional: false, reloadOnChange: true);

//Entity FrameWork
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); ;

//Services
builder.Services.AddScoped<ISupplierService, SupplierService>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<ModelStateExceptionFilter>();

});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
