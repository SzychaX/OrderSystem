namespace dotNET.Models;

public class OrderProduct
{
    public int OrderId { get; set; } // Klucz obcy do zamówienia
    
    [System.Text.Json.Serialization.JsonIgnore] // Ignorowanie podczas serializacji/deserializacji
    public Order? Order { get; set; } = null!;// Opcjonalne, wymagane tylko podczas odczytu

    public int ProductId { get; set; } // Klucz obcy do produktu
    public Product? Product { get; set; } // Opcjonalne, wymagane tylko podczas odczytu
}