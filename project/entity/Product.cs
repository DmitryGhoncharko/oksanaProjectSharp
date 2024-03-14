namespace project.entity;

// Класс для представления товара
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public int Quantity { get; set; }
    public int MinQuantity { get; set; }
}