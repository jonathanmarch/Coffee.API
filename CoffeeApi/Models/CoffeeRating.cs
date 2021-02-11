namespace Coffee.API.Models
{
    public class CoffeeRating
    {
        public int Id { get; set; }

        public int CoffeeId { get; set; }

        public string Comment { get; set; }
        
        public int Rating { get; set; }
    }
}
