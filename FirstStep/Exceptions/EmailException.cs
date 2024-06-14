namespace FirstStep.Exceptions
{
    public class EmailException : Exception
    {
        public EmailException(string message) : base(message) { }
    }

    public class EmailAlreadyExistsException : EmailException
    {
        public EmailAlreadyExistsException(string message) : base(message) { }
    }

    public class RegistrationNumberAlreadyExistsException : EmailException
    {
        public RegistrationNumberAlreadyExistsException(string message) : base(message) { }
    }
}
