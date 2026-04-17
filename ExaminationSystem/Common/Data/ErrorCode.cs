namespace ExaminationSystem.Common.Data
{
    public enum ErrorCode
    {
        NoError = 0,
        NotFound = 1,
        AlreadyExists = 2,
        NotAvailable = 3,
        InvalidData = 4,
        Unauthorized = 5,
        DatabaseError = 6,
        AttemptLimitReached = 7,
        Forbidden = 8,
        Conflict = 9,
        Gone = 10
    }
}
