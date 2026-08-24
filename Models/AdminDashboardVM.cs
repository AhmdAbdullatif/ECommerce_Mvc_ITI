namespace ECommerce_Mvc.Models
{
        public class AdminDashboardVM
        {
            public int TotalCustomers { get; set; }
            public int TotalSellers { get; set; }
            public int TotalProducts { get; set; }
            public int TotalOrders { get; set; }
            public int PendingOrders { get; set; }
        }
    }