namespace dotNET.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Domyślna data zamówienia
    public int UserId { get; set; } // Klucz obcy dla użytkownika
    public User? User { get; set; } // Opcjonalne, wymagane tylko podczas odczytu zamówienia
    public List<OrderProduct> OrderProducts { get; set; } = new(); // Inicjalizowana domyślnie
}