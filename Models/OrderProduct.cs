﻿namespace dotNET.Models;

public class OrderProduct
{
    public int Id { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}