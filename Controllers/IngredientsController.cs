using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace RestApi.Controllers;

[Route("[controller]")]
[ApiController]
public class IngredientsController
{
    private readonly IRepository<Ingredient> Ingredients;

    public IngredientsController(IRepository<Ingredient> ingredients)
    {
        Ingredients = ingredients;
    }
    
    
    [HttpGet]
    public JsonResult Get()
    {
        return new JsonResult(Ingredients.GetAll());
    }
    
    [HttpGet("{id}")]
    public JsonResult Get(int id)
    {
        return new JsonResult(Ingredients.Get(id));
    }

    [HttpPost]
    public JsonResult Post(string name,  double quantity, string measure)
    {
        if (Ingredients.GetAll().Where(x => x.Name == name).FirstOrDefault() != default)
            return new JsonResult("Error: BD have dish with this Name");
        var ingredient = new Ingredient() {Name = name, Quantity = quantity,Measure = measure};
        Ingredients.Create(ingredient);
        return new JsonResult("Dish added in BD");
    }
    
    [HttpPut("{id}")]
    public JsonResult Put(int id, [FromForm] IngredientView ingredientView)
    {
        var ingredient = new Ingredient()
        {
            Id = id,
            Name = ingredientView.Name, 
            Quantity = ingredientView.Quantity,
            Measure = ingredientView.Measure
        };
        Ingredients.Update(ingredient);
        return new JsonResult("Dish update in BD");
    }

    [HttpDelete]
    public JsonResult Delete(int id)
    {
        Ingredients.Delete(id);
        return new JsonResult("Dish was delete from BD");
    }
}