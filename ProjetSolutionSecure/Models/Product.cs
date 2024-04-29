using System.ComponentModel.DataAnnotations;
using System;
namespace ProjetSolutionSecure.Models { }
public class Product
{
    public int Id { get; set; }
    [StringLength(255)] public string Title { get; set; }
    public string Description { get; set; }
    public string Manufacturer { get; set; }
    public decimal Price { get; set; }
    public string? AdditionalInformation { get; set; }
    public ProductType Type { get; set; }
}

public enum ProductType
{
    Carrosserie,
    Peinture,
    Moteur
}