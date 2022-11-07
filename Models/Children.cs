using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models;

public class Children : BaseModel
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Patronymic { get; set; }
    public int GradeID { get; set; }
    public int ParentID { get; set; }
}

public class ChildrenData : BaseModel
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Patronymic { get; set; }
    public Grade Grade { set; get; }
    public User Parent { set; get; }
}

public class ChildrenView
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Patronymic { get; set; }
    public GradeView Grade { set; get; }
    public UserView Parent { set; get; }

    public ChildrenView(Children children, Grade grade, User teacher, User parent)
    {
        FirstName = children.FirstName;
        SecondName = children.SecondName;
        Patronymic = children.Patronymic;
        Grade = new GradeView(grade, teacher);
        Parent = new UserView(parent);
    }
}

public class ChildrenInfo : BaseModel
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Patronymic { get; set; }

    public ChildrenInfo(Children children)
    {
        Id = children.Id;
        FirstName = children.FirstName;
        SecondName = children.SecondName;
        Patronymic = children.Patronymic;
    }
}