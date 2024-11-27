namespace dotNET.Models;

public class OrderProduct
{
    public int OrderId { get; set; } 
    
    [System.Text.Json.Serialization.JsonIgnore] // Ignorowanie podczas serializacji/deserializacji
    public Order? Order { get; set; } = null!;

    public int ProductId { get; set; } 
    public Product? Product { get; set; } 
}