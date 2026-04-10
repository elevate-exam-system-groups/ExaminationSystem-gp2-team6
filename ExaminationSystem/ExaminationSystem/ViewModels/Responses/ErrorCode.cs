namespace ExamCore.Presentation.ViewModels.Responses
{
    public enum ErrorCode
    {   
        None = 0,


        // 100 → Authentication & Identity
        EmailAlreadyRegistered = 100,
        InvalidCredentials = 101,
        UserNotFound = 102,
        AccountNotVerified = 103,
        AccountLocked = 104,
        InvalidOtp = 105,
        OtpExpired = 106,
        TooManyOtpRequests = 107,
        InvalidResetToken = 108,
        PasswordPolicyViolation = 109,


        // 200 → Authorization
        Unauthorized = 200,
        Forbidden = 201,
        InvalidRole = 202,


        // 300 → Student
        StudentNotFound = 300,
        StudentNotEnrolled = 301,
        StudentAlreadyEnrolled = 302,


        // 400 → Diploma
        DiplomaNotFound = 400,
        DiplomaNotPublished = 401,
        DiplomaHasActiveEnrollments = 402,


        // 500 → Quiz
        QuizNotFound = 500,
        QuizNotPublished = 501,
        AttemptLimitReached = 502,
        QuizAlreadyPublished = 503,
        QuizHasNoQuestions = 504,


        // 600 → Attempt
        AttemptNotFound = 600,
        AttemptAlreadySubmitted = 601,
        AttemptNotOwned = 602,
        AttemptExpired = 603,
        AttemptInProgress = 604,


        // 700 → Question
        QuestionNotFound = 700,
        InvalidQuestion = 701,
        QuestionNotInQuiz = 702,


        // 800 → Choice
        ChoiceNotFound = 800,
        InvalidChoice = 801,

 
        // 900 → Validation & General
        ValidationError = 900,
        InvalidRequest = 901,
        MissingRequiredField = 902,
        UnsupportedMediaType = 903


    }
}