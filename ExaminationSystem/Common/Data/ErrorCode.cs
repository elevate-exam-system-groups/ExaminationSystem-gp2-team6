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
        Gone = 10,
        ValidationError = 11,
        Unknown = 12,
        UserNotFound = 13,
        UserAlreadyExists = 14,
        InvalidCredentials = 15, // IncorrectPassword
        TooManyRequests = 16,
        UserLocked = 17,
        VerificationCodeExpired = 18,
        VerificationCodeInvalid = 19,
        PasswordNotMatch = 20,
        ExpiredTemporaryToken = 21
    }
}