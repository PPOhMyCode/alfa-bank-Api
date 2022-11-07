﻿using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace RestApi.Controllers;

[Route("[controller]")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IRepository<Dish> Dishes;
    private readonly IRepository<Ingredient> Ingredient;
    private readonly IRepository<Dish_Ingredient> Dish_Ingredient;
    private readonly IRepository<SummaryOrder> SummaryOrder;

    public DishController(IRepository<Dish> dishes,
        IRepository<Ingredient> ingredient, 
        IRepository<Dish_Ingredient> dish_Ingredient,
        IRepository<SummaryOrder> summaryOrder)
    {
        Dishes = dishes;
        Ingredient = ingredient;
        Dish_Ingredient = dish_Ingredient;
        SummaryOrder = summaryOrder;
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
            Dish = new DishView(dish),
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
    public JsonResult Post( [FromForm]DishView dishView)
    {
        if (Dishes.GetAll().FirstOrDefault(x => x.Name == dishView.Name) != default)
            return new JsonResult("Error: BD have dish with this Name");
        var dish = new Dish() 
        {
            Name = dishView.Name, 
            Discription = dishView.Discription, 
            Calories = dishView.Calories, 
            Cost = dishView.Cost, 
            Weight = dishView.Weight
        };
        Dishes.Create(dish);
        return new JsonResult("Dish added in BD");
    }
    
    [HttpPut("{id}")]
    public JsonResult Put(int id, [FromForm]DishView dishView)
    {
        var dish = new Dish()
        {
            Id = id,
            Name = dishView.Name,
            Discription = dishView.Discription,
            Calories = dishView.Calories,
            Weight = dishView.Weight,
            Cost = dishView.Cost
        };
        Dishes.Update(dish);
        return true ? new JsonResult($"Update successful {id}") : new JsonResult("Update was not successful");
    }

    [HttpDelete]
    public JsonResult Delete(int id)
    {
        Dishes.Delete(id);
        return new JsonResult("Dish was delete from BD");
    }
}