namespace Coffee.API
{
    public class Constants
    {
        public static class RouteConstants
        {
            public const string GetCoffee = "{coffeeId}";

            public const string UpdateCoffee = "{coffeeId}";

            public const string DeleteCoffee = "{coffeeId}";

            public const string UpdateCoffeeRating = "{coffeeRatingId}";
        }

        public static class ErrorMessages
        {
            public const string AnErrorOccurred = "Sorry an error error occurred while trying to process your request.";
        }
    }
}
