namespace Pronia.Models
{
    public class Basket:BaseEntity
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public ICollection<ProductBasket> ProductBasket { get; set; }

        public int ProductCount { get; set; }
    }
}
