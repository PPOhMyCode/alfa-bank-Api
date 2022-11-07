namespace MyApi.Models;

public interface IRepository<DBModel> where DBModel : BaseModel
{
    public List<DBModel> GetAll();
    public DBModel Get(int id);
    public DBModel Create(DBModel model);
    public DBModel Update(DBModel model);
    public void Delete(int id);
}