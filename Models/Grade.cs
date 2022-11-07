using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models;

public class Grade: BaseModel
{
    public string Name { get; set; }
    public int TeacherID { get; set; }
}

public class GradeData : BaseModel
{
    public string Name { get; set; }
    public User Teacher { get; set; }
}

public class GradeView 
{
    public string Name { get; set; }
    public UserView Teacher { get; set; }

    public GradeView(Grade grade,User teacher)
    {
        Name = grade.Name;
        Teacher = new UserView(teacher);
    }
}
