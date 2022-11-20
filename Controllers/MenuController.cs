using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyApi.Models;

namespace RestApi.Controllers;

[Route("[controller]")]
[ApiController]
public class MenuController: ControllerBase
{
    private readonly IRepository<Menu> Menu;
    private readonly IRepository<Dish> Dish;
    private readonly IRepository<TypeMeal> TypeMeal;

    public MenuController(IRepository<Menu> menu,
        IRepository<Dish> dish, IRepository<TypeMeal> typeMeal)
    {
        Menu = menu;
        Dish = dish;
        TypeMeal = typeMeal;
    }
    
    [HttpGet]
    public JsonResult Get()
    {
        return new JsonResult(Menu.GetAll());
    }
    
    [HttpGet("{id}")]
    public JsonResult Get(int id)
    {
        return new JsonResult(Menu.Get(id));
    }
    
    [HttpGet("Date/{date}")]
    public JsonResult GetDate(DateTime date)
    {
        return new JsonResult(Menu.GetAll().Where(x=>x.Date==date));
    }
    
    [HttpGet("TypeMeal/{id}")]
    public JsonResult GetTypeMeal(int id)
    {
        return new JsonResult(Menu.GetAll().Where(x=>x.TypeMealId==id));
    }

    [HttpPost]
    public JsonResult Post(MenuInput menuInput)
    {
        if (Menu.GetAll().FirstOrDefault(x => x.Date == menuInput.Date 
                                              && x.DishId == menuInput.DishId 
                                              && x.TypeMealId == menuInput.TypeMealId) != default)
            return new JsonResult("Error: BD have this dish in menu");
        
        var menu = new Menu()
        {
            Date = menuInput.Date,
            DishId = menuInput.DishId,
            TypeMealId = menuInput.TypeMealId
        };
        Menu.Create(menu);
        return new JsonResult("Dish added in Menu");
    }
    
    [HttpDelete("{id}")]
    public JsonResult Delete(int id)
    {
        Menu.Delete(id);
        return new JsonResult("Dish was delete from Menu");
    }
}