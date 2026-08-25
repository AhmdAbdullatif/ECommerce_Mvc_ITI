namespace ECommerce_Mvc.Configuration;

public class StripeOptions
{
    public const string  SectionName = "Stripe";
    public string SecretKey { get; set; } = null!;
    public string Domain { get; set; } = null!;
}