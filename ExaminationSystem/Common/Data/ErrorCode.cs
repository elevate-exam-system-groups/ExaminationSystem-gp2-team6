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
<<<<<<< HEAD
        UserNotFound = 6,
        UserAlreadyExists = 7,
        InvalidCredentials = 8, // IncorrectPassword
        TooManyRequests = 9,
        UserLocked = 10,
        ValidationError = 11,
        VerificationCodeExpired = 12,
        VerificationCodeInvalid = 13,
        PasswordNotMatch = 14,
        ExpiredTemraryToken = 15
=======
        DatabaseError = 6,
        AttemptLimitReached = 7,
        Forbidden = 8,
        Conflict = 9,
        Gone = 10,
        ValidationError = 11,
        Unknown = 12
>>>>>>> Testing
    }
}
