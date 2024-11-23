namespace dotNET.Models;

public class OrderProduct
{
    public int OrderId { get; set; } // Klucz obcy do zamówienia
    public Order? Order { get; set; } // Opcjonalne, wymagane tylko podczas odczytu

    public int ProductId { get; set; } // Klucz obcy do produktu
    public Product? Product { get; set; } // Opcjonalne, wymagane tylko podczas odczytu
}