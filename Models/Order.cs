namespace dotNET.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow; 
    public int UserId { get; set; } 
    public User? User { get; set; }

    public List<OrderProduct> OrderProducts { get; set; } = new(); 
}