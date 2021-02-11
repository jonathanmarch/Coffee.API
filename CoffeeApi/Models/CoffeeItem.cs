using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Coffee.API.Models
{
    public class CoffeeItem
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int CaffeineContent { get; set; }

        [JsonIgnore]
        public List<CoffeeRating> Ratings { get; set; } = new List<CoffeeRating>();

        public double? AverageRating
        {
            get
            {
                if (!this.Ratings.Any())
                {
                    return null;
                }
                
                return this.Ratings.Select(i => i.Rating).Average();
            }
        }

        public int totalRatings
        {
            get => this.Ratings.Count;
        }

        public IEnumerable<string> Comments
        {
            get => this.Ratings.Select(i => i.Comment);
        }
    }
}
