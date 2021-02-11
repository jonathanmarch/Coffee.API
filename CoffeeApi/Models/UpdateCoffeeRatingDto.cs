namespace Coffee.API.Models
{
    public class UpdateCoffeeRatingDto
    {
        public int Id { get; set; }
        
        public string Comment { get; set; }

        public int Rating { get; set; }
    }
}
