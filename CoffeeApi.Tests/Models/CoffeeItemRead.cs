using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeApi.Tests.Models
{
    public class CoffeeItemRead
    {
        public int Id { get; set;  }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int CaffeineContent { get; set; }

        public double? AverageRating { get; set; }

        public int TotalRatings { get; set; }
        
        public IEnumerable<string> Comments { get; set; }
    }
}
