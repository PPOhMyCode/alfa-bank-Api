using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace RestApi.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IRepository<Order> Orders;
    private readonly IRepository<StatusOrder> StatusOrder;
    private readonly IRepository<Children> Children;
    private readonly IRepository<SummaryOrder> SummaryOrder;
    private readonly IRepository<Dish_Ingredient> Dish_Ingredient;
    private readonly IRepository<Ingredient> Ingredient;
    private readonly IRepository<Timing> Timing;
    private readonly IRepository<TypeMeal> TypeMeal;
    private readonly IRepository<Grade> Grade;
    private readonly IRepository<User> Users;
    private readonly IRepository<Dish> Dish;
    private readonly IRepository<Role> Role;

    public OrdersController(IRepository<Order> orders,
        IRepository<StatusOrder> statusOrder,
        IRepository<SummaryOrder> summaryOrder,
        IRepository<Timing> timing,
        IRepository<Children> children,
        IRepository<TypeMeal> typeMeal,
        IRepository<Grade> grade,
        IRepository<User> users,
        IRepository<Dish> dish,
        IRepository<Role> role,
        IRepository<Dish_Ingredient> dishIngredient,
        IRepository<Ingredient> ingredient)
    {
        Orders = orders;
        StatusOrder = statusOrder;
        SummaryOrder = summaryOrder;
        Timing = timing;
        Children = children;
        TypeMeal = typeMeal;
        Grade = grade;
        Users = users;
        Dish = dish;
        Role = role;
        Dish_Ingredient = dishIngredient;
        Ingredient = ingredient;
    }
    
    private DishIngredientView GetDishIngredientView(int id)
    {
        var dish = Dish.Get(id);
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

    
    private SummaryOrderView GetFullSummaryOrder(SummaryOrder summaryOrder)
    {
        var order = Orders.Get(summaryOrder.OrderId);
        var statusOrder = StatusOrder.Get(order.StatusId);
        var timing = Timing.Get(summaryOrder.TimingId);
        var typeMeal = TypeMeal.Get(timing.TypeId);
        var children = Children.Get(order.ChildrenId);
        var grade = Grade.Get(children.GradeID);
        var dish = Dish.Get(order.DishId);
        var teacher = Users.Get(grade.TeacherID);
        var result = new SummaryOrderView()
        {
            Id = statusOrder.Id,
            Count = summaryOrder.Count,
            Date = summaryOrder.Date,
            StatusOrder = statusOrder.Name,
            Time = timing.Time,
            TypeMeal = typeMeal.Name,
            Grade = new GradeView(grade,teacher),
            Children = new ChildrenInfo(children),
            Dish = dish
        };
        return result;
    }

    [HttpGet]
    public JsonResult GetAll()
    {
        return new JsonResult(SummaryOrder.GetAll().Select(GetFullSummaryOrder));
    }
    
    [HttpGet("Date/{date}")]
    public JsonResult GetToday(DateTime date)
    {
        return new JsonResult(SummaryOrder.GetAll().Where(x=>x.Date == date).Select(GetFullSummaryOrder));
    }
    
    [HttpGet("Date/{date}/Ingredients")]
    public JsonResult GetTodayIngredients(DateTime date)
    {
        var summaryOrders = SummaryOrder.GetAll().Where(x => x.Date == date).Select(GetFullSummaryOrder);
        var result = new List<DishIngredientView>();
        var listDishIds = new List<int>();
        foreach (var summaryOrder in summaryOrders)
        {
            if (listDishIds.Contains(summaryOrder.Dish.Id))
            {
                result.FirstOrDefault(x => x.Dish.Id == summaryOrder.Dish.Id)!.CountOrders+=summaryOrder.Count;
            }
            else
            {
                listDishIds.Add(summaryOrder.Dish.Id);
                var dishIngredient = GetDishIngredientView(summaryOrder.Dish.Id);
                
                result.Add(new DishIngredientView()
                {
                    CountOrders = summaryOrder.Count,
                    Dish = dishIngredient.Dish,
                    Ingredients = dishIngredient.Ingredients
                });    
            }
            
        }
        return new JsonResult(result);
    }
    
    [HttpGet("Children/{id}")]
    public JsonResult GetChildren(int id)
    {
        return new JsonResult(SummaryOrder.GetAll().Select(GetFullSummaryOrder).Where(x=>x.Children.Id == id));
    }
    
    [HttpGet("Children/{id}/{date}")]
    public JsonResult GetChildrenToday(int id,DateTime date)
    {
        return new JsonResult(SummaryOrder.GetAll().Select(GetFullSummaryOrder).Where(x=>x.Children.Id == id && x.Date == date));
    }
    
    [HttpGet("{id}")]
    public JsonResult Get(int id)
    {
        var summaryOrder = SummaryOrder.Get(id);
        return new JsonResult(GetFullSummaryOrder(summaryOrder));
    }
    
    
    
    [HttpDelete("{id}")]
    public JsonResult Delete(int id)
    {
        var sumorder = SummaryOrder.Get(id);
        var order = Orders.Get(sumorder.OrderId);
        SummaryOrder.Delete(id);
        Orders.Delete(order.Id);
        return new JsonResult($"Order {id} was be delete");
    }
    
    [HttpPut("{id}/Status")]
    public JsonResult PutStatus(int id, int statusId)
    {
        var summaryOrder = SummaryOrder.Get(id);
        if (summaryOrder == default || statusId>StatusOrder.GetAll().Count)
            return new JsonResult("Update was not successful");
        var order = Orders.Get(summaryOrder.OrderId);
        order.StatusId = statusId;
        Orders.Update(order);
        return new JsonResult("Update was successful");
    }
    
    [HttpPut("{id}/Count")]
    public JsonResult PutCount(int id, int count)
    {
        if(count <=0)
            return new JsonResult("Update was not successful. Count <= 0 use Delete");
        var summaryOrder = SummaryOrder.Get(id);
        summaryOrder.Count = count;
        SummaryOrder.Update(summaryOrder);
        return new JsonResult("Update was successful");
    }


    [HttpPost]
    public JsonResult Post(OrderInput orderInput)
    {
        if (SummaryOrder
                .GetAll()
                .FirstOrDefault(x => x.Date == orderInput.DateTime 
                                     && Orders.Get(x.OrderId).ChildrenId == orderInput.ChildrenId
                                     && Orders.Get(x.OrderId).TypeId == orderInput.TypeId) 
            != default)
            return new JsonResult("Error: BD have this order");
        var order = new Order()
        {
            ChildrenId = orderInput.ChildrenId,
            DishId = orderInput.DishId,
            TypeId = orderInput.TypeId,
            StatusId = 1
        };
        Orders.Create(order);
        var children = Children.GetAll().FirstOrDefault(x => x.Id == orderInput.ChildrenId);
        var timing = Timing.GetAll().FirstOrDefault(x => x.TypeId == orderInput.TypeId && x.GradleId == children.GradeID);
       
        var summaryOrder = new SummaryOrder()
        {
            TimingId = timing.Id,
            OrderId = order.Id,
            Date = orderInput.DateTime,
            Count = 1
        };
        SummaryOrder.Create(summaryOrder);
        return new JsonResult("Order added in BD");
    }
}