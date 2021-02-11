namespace CoffeeApi.Tests.Models
{
    public class CoffeeRatingRead
    {
        public int Id { get; set; }

        public int CoffeeId { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }
    }
}
