using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models;

public class Timing : BaseModel
{
    public TimeSpan Time { get; set; }
    public int TypeId { get; set; }
    public int GradleId { get; set; }
}

public class TimingData : BaseModel
{
    public TimeSpan Time { get; set; }
    public TypeMeal TypeMeal { get; set; }
    public Grade Grade { get; set; }
}

public class TimingView
{
    public TimeSpan Time { get; set; }
    public string TypeMeal { get; set; }
    public GradeView Grade { get; set; }

    public TimingView(TimeSpan time, TypeMeal typeMeal, Grade grade)
    {
        Time = time;
        TypeMeal = typeMeal.Name;
    }
}