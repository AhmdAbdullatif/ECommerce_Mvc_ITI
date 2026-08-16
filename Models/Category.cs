namespace ECommerce_Mvc.Models;

public class Category
{
    private Category() { } // Required by EF Core

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];

    public Category(string name)
    {
        Name = name;
    }
}
