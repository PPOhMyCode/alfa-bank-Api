using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace RestApi.Controllers;

[Route("[controller]")]
[ApiController]
public class DefaultMenuController: ControllerBase
{
    private readonly IRepository<DefaultMenu> DefaultMenu;
    private readonly IRepository<Dish> Dish;
    private readonly IRepository<TypeMeal> TypeMeal;

    public DefaultMenuController(IRepository<DefaultMenu> defaultMenu,
        IRepository<Dish> dish, IRepository<TypeMeal> typeMeal)
    {
        DefaultMenu = defaultMenu;
        Dish = dish;
        TypeMeal = typeMeal;
    }
    
    [HttpGet]
    public JsonResult Get()
    {
        return new JsonResult(DefaultMenu.GetAll());
    }
    
    [HttpGet("{id}")]
    public JsonResult Get(int id)
    {
        return new JsonResult(DefaultMenu.Get(id));
    }
    
    [HttpGet("NumMenu/{num}")]
    public JsonResult GetDate(int num)
    {
        return new JsonResult(DefaultMenu.GetAll().Where(x=>x.NumMenu==num));
    }
    
    [HttpGet("TypeMeal/{id}")]
    public JsonResult GetTypeMeal(int id)
    {
        return new JsonResult(DefaultMenu.GetAll().Where(x=>x.TypeMealId==id));
    }

    [HttpPost]
    public JsonResult Post(DefaultMenuInput menuInput)
    {
        if (DefaultMenu.GetAll().FirstOrDefault(x => x.NumMenu == menuInput.NumMenu 
                                                     && x.DishId == menuInput.DishId 
                                                     && x.TypeMealId == menuInput.TypeMealId) != default)
            return new JsonResult("Error: BD have this dish in menu");
        
        var menu = new DefaultMenu()
        {
            NumMenu = menuInput.NumMenu,
            DishId = menuInput.DishId,
            TypeMealId = menuInput.TypeMealId
        };
        DefaultMenu.Create(menu);
        return new JsonResult("Dish added in DefaultMenu №"+menu.NumMenu);
    }
    
    [HttpDelete]
    public JsonResult Delete(int id)
    {
        var menu = DefaultMenu.Get(id);
        DefaultMenu.Delete(id);
        return new JsonResult("Dish was delete from  №"+menu.NumMenu);
    }
}