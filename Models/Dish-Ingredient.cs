using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models;

public class Dish_Ingredient : BaseModel
{
    public int DishID { set; get; }
    public int IngredientID { set; get; }
    public double Count { set; get; }
}

public class DishIngredientView
{
    public Dish Dish { set; get; }
    public int CountOrders { get; set; }
    public List<IngredientCount>  Ingredients { set; get; }
}

