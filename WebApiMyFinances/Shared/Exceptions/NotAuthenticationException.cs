namespace WebApiMyFinances.Shared.Exceptions
{
    public class NotAuthenticationException : Exception
    {
        public NotAuthenticationException(string message) : base(message)
        {

        }
    }
}
