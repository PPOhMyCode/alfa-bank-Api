using Microsoft.EntityFrameworkCore;

namespace MyApi.Models;

public class Repository<DBModel>: IRepository<DBModel> where DBModel: BaseModel
{
    private ApplicationContext Context { get; set; }
    public Repository(ApplicationContext context)
    {
        Context = context;
    }
    public List<DBModel> GetAll()
    {
        return Context.Set<DBModel>().ToList();
    }

    public DBModel Get(int id)
    {
        return Context.Set<DBModel>().FirstOrDefault(model => model.Id == id);
    }
    
    public DBModel Create(DBModel model)
    {
        Context.Set<DBModel>().Add(model);
        Context.SaveChanges();
        return model;
    }

    public DBModel Update(DBModel model)
    {
        var toUpdate = Context.Set<DBModel>().FirstOrDefault(m => m.Id == model.Id);
        if (toUpdate != null)
        {
            toUpdate = model;
        }
        
        Context.Set<DBModel>().Update(toUpdate);
        Context.SaveChanges();
        return toUpdate;
    }

    public void Delete(int id)
    {
        var toDelete = Context.Set<DBModel>().FirstOrDefault(m => m.Id == id);
        Context.Set<DBModel>().Remove(toDelete);
        Context.SaveChanges();
    }
}