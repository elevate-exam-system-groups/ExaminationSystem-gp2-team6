namespace ExaminationSystem.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public object? ExistingResult { get; }

        public ConflictException(string message, object? existingResult = null)
            : base(message)
        {
            ExistingResult = existingResult;
        }
    }
}
