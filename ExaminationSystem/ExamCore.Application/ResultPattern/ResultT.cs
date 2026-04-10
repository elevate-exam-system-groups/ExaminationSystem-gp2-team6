using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Application.ResultPattern
{
    public class ResultT<T> : Result where T : class
    {
        public T Data { get; }

        private ResultT(bool isSuccess, T value, Error error)
            : base(isSuccess, error)
        {
            Data = value;
        }

        public static ResultT<T> Success(T value)
            => new(true, value, Error.None);

        public static ResultT<T> Failure(Error error)
            => new(false, default!, error);
    }
}
