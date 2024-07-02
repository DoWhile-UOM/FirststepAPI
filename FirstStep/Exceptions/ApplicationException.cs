namespace FirstStep.Exceptions
{
    // Base exception class, if you want to extend from it in the future
    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message) { }
    }

    // Custom exception for no applications to evaluate
    public class NoApplicationsToEvaluateException : ApplicationException
    {
        public NoApplicationsToEvaluateException(string message) : base(message) { }
    }

    // Custom exception for not enough HR assistants
    public class NotEnoughHRAssistantsForEvaluationException : ApplicationException
    {
        public NotEnoughHRAssistantsForEvaluationException(string message) : base(message) { }
    }

    // Custom exception for successful task delegation
    public class SuccessfulTaskDelegationException : ApplicationException
    {
        public SuccessfulTaskDelegationException(string message) : base(message) { }
    }

    public class ApplicationNotFoundException : ApplicationException
    {
        public ApplicationNotFoundException(string message) : base(message) { }
    }

    public class ApplicationAlreadyExistsException : ApplicationException
    {
        public ApplicationAlreadyExistsException(string message) : base(message) { }
    }
}

