namespace ECommerce_Mvc.Models;

public class Address
{
    private Address() { } // Required by EF Core

    public string Country { get; private set; } = null!;
    public string State { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string Street { get; private set; } = null!;
    public Address(string country, string state, string city, string street)
    {
        Country = country;
        State = state;
        City = city;
        Street = street;
    }

    public Address(string city, string street)
    {
        City = city;
        Street = street;
    }
}
