using System;

namespace Coffee.API.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            : base("Unable to find entity.")
        {
        }
    }
}
