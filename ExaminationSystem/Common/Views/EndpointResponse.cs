using ExaminationSystem.Common.Data;

namespace ExaminationSystem.Common.Views
{
    public class EndpointResponse<T>(T Data,bool IsSuccess,string Message,ErrorCode ErrorCode)
    {
        public static EndpointResponse<T> Success(T data, string message = "")
            => new(data, true, message, ErrorCode.NoError);
        public static EndpointResponse<T> Failure(ErrorCode errorCode, string message = "")
            => new(default!, false, message, errorCode);
    }
}
