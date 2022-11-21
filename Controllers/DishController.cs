using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace RestApi.Controllers;

[Route("[controller]")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IRepository<Dish> Dishes;
    private readonly IRepository<Ingredient> Ingredient;
    private readonly IRepository<DefaultMenu> DefaultMenu;
    private readonly IRepository<Menu> Menu;
    private readonly IRepository<Order> Orders;
    private readonly IRepository<Dish_Ingredient> Dish_Ingredient;
    private readonly IRepository<SummaryOrder> SummaryOrder;

    public DishController(IRepository<Dish> dishes,
        IRepository<Ingredient> ingredient, 
        IRepository<Dish_Ingredient> dish_Ingredient,
        IRepository<DefaultMenu> defaultMenu,
        IRepository<Menu> menu,
        IRepository<Order> order,
        IRepository<SummaryOrder> summaryOrder)
    {
        Dishes = dishes;
        Ingredient = ingredient;
        Dish_Ingredient = dish_Ingredient;
        SummaryOrder = summaryOrder;
        DefaultMenu = defaultMenu;
        Menu = menu;
        Orders = order;
    }
    
    private DishIngredientView GetDishIngredientView(int id)
    {
        var dish = Dishes.Get(id);
        var dishIngredients = Dish_Ingredient.GetAll().Where(x => x.DishID == id);
        var Ingredients = new List<IngredientCount>();
        foreach (var dishIngredient in dishIngredients)
        {
            Ingredients.Add(new IngredientCount()
            {
                Count = dishIngredient.Count,
                Ingredient = new IngredientView(Ingredient.Get(dishIngredient.IngredientID))
            });
        }
        DishIngredientView result = new DishIngredientView()
        {
            Dish = dish,
            CountOrders = 0,
            Ingredients = Ingredients
        };
        
        return result;
    }

    [HttpGet]
    public JsonResult Get()
    {
        return new JsonResult(Dishes.GetAll());
    }
    
    [HttpGet("{id}")]
    public JsonResult Get(int id)
    {
        return new JsonResult(Dishes.Get(id));
    }
    
    [HttpGet("{id}/withIngredients")]
    public JsonResult GetDishIngredientId(int id)
    {
        return new JsonResult(GetDishIngredientView(id));
    }

    [HttpPost]
    public JsonResult Post(DishInput dishInput)
    {
        if (Dishes.GetAll().FirstOrDefault(x => x.Name == dishInput.Name) != default)
            return new JsonResult("Error: BD have dish with this Name");
        var dish = new Dish() 
        {
            Name = dishInput.Name, 
            Discription = dishInput.Discription, 
            Calories = dishInput.Calories, 
            Cost = dishInput.Cost, 
            Weight = dishInput.Weight,
            Carbohydrates = dishInput.Carbohydrates,
            Fats = dishInput.Fats,
            Proteins = dishInput.Proteins
            
        };
        Dishes.Create(dish);
        return new JsonResult("Dish added in BD");
    }
    
    [HttpPost("{id}/ingredients")]
    public JsonResult Post(int id, List<IngredientCountInput> ingredientsIdCount)
    {
        if (Dish_Ingredient.GetAll().FirstOrDefault(x => x.DishID == id) != default)
            return new JsonResult("Error: Dish have ingredients, please ue Delete or Put");
        foreach (var ingredient in ingredientsIdCount)
        {
            var dish_ingredient = new Dish_Ingredient()
            {
                DishID = id,
                IngredientID = ingredient.IngredientId,
                Count = ingredient.Count
            };
            Dish_Ingredient.Create(dish_ingredient); 
        }
        return new JsonResult("Ingredients added in BD");
    }
    
    [HttpPut("{id}")]
    public JsonResult Put(int id, DishInput dishInput)
    {
        var dish = new Dish()
        {
            Id = id,
            Name = dishInput.Name,
            Discription = dishInput.Discription,
            Calories = dishInput.Calories,
            Weight = dishInput.Weight,
            Cost = dishInput.Cost,
            Proteins = dishInput.Proteins,
            Carbohydrates = dishInput.Carbohydrates,
            Fats = dishInput.Fats
        };
        Dishes.Update(dish);
        return true ? new JsonResult($"Update successful {id}") : new JsonResult("Update was not successful");
    }

    [HttpDelete("{id}")]
    public JsonResult Delete(int id)
    {
        var dish_ingredients = Dish_Ingredient.GetAll().Where(x => x.DishID == id);
        if(dish_ingredients!=null)
            foreach (var dish_ingredient in dish_ingredients)
                Dish_Ingredient.Delete(dish_ingredient.Id);
        
        var defaultMenus = DefaultMenu.GetAll().Where(x => x.DishId == id);
        if(defaultMenus!=null)
            foreach (var defaultMenu in defaultMenus)
                DefaultMenu.Delete(defaultMenu.Id);
        
        var menus = Menu.GetAll().Where(x => x.DishId == id);
        System.Console.WriteLine(menus.FirstOrDefault());
        if(menus!=null)
            foreach (var menu in menus)
                Menu.Delete(menu.Id);
        
        var orders = Orders.GetAll().Where(x => x.DishId == id);
        if(orders!=null)
            foreach (var order in orders)
                Orders.Delete(order.Id);
        
        Dishes.Delete(id);
        
        return new JsonResult("Dish was delete from BD");
    }
}