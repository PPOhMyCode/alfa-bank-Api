using Microsoft.AspNetCore.Mvc;
using MyApi;
using MyApi.Models;

namespace RestApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AutorizationController : ControllerBase
{
    private readonly IRepository<User> Users;
    private readonly IRepository<Role> Roles;

    public AutorizationController(IRepository<User> users,IRepository<Role> roles)
    {
        Users = users;
        Roles = roles;
    }
    [HttpGet]
    public JsonResult Get()
    {
        return new JsonResult(Users.GetAll());
    }
    
    [HttpPost]
    public JsonResult Post(UserAutorization userAutorization)
    {
        var user = Users.GetAll().Where(x => x.Login == userAutorization.Login).FirstOrDefault();
        if (user == default)
            return new JsonResult("No people with this login");
        if (user.Password != userAutorization.Password)
            return new JsonResult("Password is wrong");
        
        var userView = new UserDataView
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.Password,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            Patronymic = user.Patronymic,
            Role = Roles.Get(user.RoleId)
        };

        return new JsonResult(userView);
    }
}