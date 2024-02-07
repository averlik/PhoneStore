namespace PhoneStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int PhoneId { get; set; }

        public Phone Phone { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
