namespace MyApi.Models;

public class DefaultMenu:BaseModel
{
    public int NumMenu { get; set; }
    public int DishId { get; set; }
    public int TypeMealId { get; set; }
}

public class DefaultMenuInput
{
    public int NumMenu { get; set; }
    public int DishId { get; set; }
    public int TypeMealId { get; set; }
}