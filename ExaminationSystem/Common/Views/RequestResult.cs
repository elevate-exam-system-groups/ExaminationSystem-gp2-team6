using ExaminationSystem.Common.Data;

namespace ExaminationSystem.Common.Views
{
    public class RequestResult<T> (T Data, bool IsSuccess, string Message, ErrorCode ErrorCode)
    {
        public static RequestResult<T> Success(T data, string message = "")
            => new(data, true, message, ErrorCode.NoError);
        public static RequestResult<T> Success(T data)
            => new(data, true, string.Empty, ErrorCode.NoError);
        public static RequestResult<T> Failure(ErrorCode errorCode, string message = "")
            => new(default!, false, message, errorCode);
        public static RequestResult<T> Failure(ErrorCode errorCode)
            => new(default!, false, string.Empty, errorCode);
    }
}
