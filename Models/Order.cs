using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models;

public class Order: BaseModel
{
    public int ChildrenId { get; set; }
    public int DishId { get; set; }
    public int StatusId { get; set; }
    public int TypeId { get; set; }
}

public class OrderInput
{
    public int ChildrenId { get; set; }
    public int DishId { get; set; }
    public int TypeId { get; set; }
    public DateTime DateTime { get; set; }
}

public class OrderData: BaseModel
{
    public Children Children { get; set; }
    public Dish Dish  { get; set; }
    public StatusOrder StatusOrder  { get; set; }
    public TypeMeal TypeMeal { get; set; }
}

public class OrderView
{
    public ChildrenView Children { get; set; }
    public Dish Dish  { get; set; }
    public string TypeMeal { get; set; }
    public string StatusOrder  { get; set; }
}