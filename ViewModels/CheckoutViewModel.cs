namespace ECommerce_Mvc.ViewModels
{
    public class CheckoutViewModel
    {
        public string Country { get; set; } = "Egypt"; // قيمة افتراضية أو تُترك للمستخدم
        public string State { get; set; } = "Beheira";  // أو المحافظة التابعة لك
        public string City { get; set; }
        public string Street { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
