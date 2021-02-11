namespace Coffee.API.Models
{
    public class ValidationError
    {
        public string Field { get; set; }

        public string Message { get;  set; }

        public ValidationError(string field, string message)
        {
            this.Field = field;
            this.Message = message;
        }
    }
}
