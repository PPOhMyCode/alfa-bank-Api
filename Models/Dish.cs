namespace MyApi.Models;

public class Dish: BaseModel
{
    public string Name { set; get; }
    public string Discription { set; get; }
    public double Cost { set; get; }
    public double Weight { set; get; }
    public double Calories { set; get; }
}

public class DishView
{
    public string Name { set; get; }
    public string Discription { set; get; }
    public double Cost { set; get; }
    public double Weight { set; get; }
    public double Calories { set; get; }
    
}


public class SystemDishView:BaseModel
{
    public string Name { set; get; }
    public string Discription { set; get; }
    public double Cost { set; get; }
    public double Weight { set; get; }
    public double Calories { set; get; }

    public SystemDishView(Dish dish)
    {
        Id = dish.Id;
        Name = dish.Name;
        Discription = dish.Discription;
        Cost = dish.Cost;
        Weight = dish.Weight;
        Calories = dish.Calories;
    }
}