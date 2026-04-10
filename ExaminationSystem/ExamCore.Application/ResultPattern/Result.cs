using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Application.ResultPattern
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error Error { get; }
        public string Message { get; }

        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
            => new(true, Error.None);
        public static Result Success(string message)
            => new(true, Error.None);

        public static Result Failure(Error error)
            => new(false, error);
    }
}
