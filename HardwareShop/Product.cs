namespace HardwareShop;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ProductCategory Category { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int LowStockLimit { get; set; }
}