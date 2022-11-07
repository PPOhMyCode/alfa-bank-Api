using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi;

public class ApplicationContext: DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Ingredient> Ingredient => Set<Ingredient>();
    public DbSet<Children> Children => Set<Children>();
    public DbSet<Dish_Ingredient> Dish_Ingredient => Set<Dish_Ingredient>();
    public DbSet<Grade> Grade => Set<Grade>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<StatusOrder> StatusOrder => Set<StatusOrder>();
    public DbSet<Timing> Timing => Set<Timing>();
    public DbSet<TypeMeal> TypeMeal => Set<TypeMeal>();
    public DbSet<SummaryOrder> SummaryOrder => Set<SummaryOrder>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options){ }

}