using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Models;

public class SummaryOrder : BaseModel
{
    public DateTime Date { get; set; }
    public int TimingId { get; set; }
    public int OrderId { get; set; }
    
    public int Count { get; set; }
}

public class SummaryOrderInput
{
    public DateTime Date { get; set; }
    public int TimingId { get; set; }
    public int OrderId { get; set; }
    
    public int Count { get; set; }
}

public class SummaryOrderData : BaseModel
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
    public Timing Timing { get; set; }
    public Order Order { get; set; }
}

public class SummaryOrderView: BaseModel
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
    public TimeSpan Time { get; set; }
    public string TypeMeal { get; set; }
    public GradeView GradeName { get; set; }
    public string StatusOrder  { get; set; }
    public SystemDishView SystemDish  { get; set; }
    public ChildrenInfo Children { get; set; }

}