using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyApi;
using MyApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = @"Server= 192.168.68.1,1433;Database=BDF;User=sa;Password=Oh1234My5678Code!;Integrated Security=false;TrustServerCertificate=True";

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(connection);
    options.EnableSensitiveDataLogging();
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Dish>, Repository<Dish>>();
builder.Services.AddScoped<IRepository<Role>, Repository<Role>>();
builder.Services.AddScoped<IRepository<Ingredient>, Repository<Ingredient>>();
builder.Services.AddScoped<IRepository<Dish_Ingredient>, Repository<Dish_Ingredient>>();
builder.Services.AddScoped<IRepository<Children>, Repository<Children>>();
builder.Services.AddScoped<IRepository<Grade>, Repository<Grade>>();
builder.Services.AddScoped<IRepository<Order>, Repository<Order>>();
builder.Services.AddScoped<IRepository<StatusOrder>, Repository<StatusOrder>>();
builder.Services.AddScoped<IRepository<SummaryOrder>, Repository<SummaryOrder>>();
builder.Services.AddScoped<IRepository<Timing>, Repository<Timing>>();
builder.Services.AddScoped<IRepository<TypeMeal>, Repository<TypeMeal>>();
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

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationContext>(); 
    context.Database.Migrate();
}

app.Run();

