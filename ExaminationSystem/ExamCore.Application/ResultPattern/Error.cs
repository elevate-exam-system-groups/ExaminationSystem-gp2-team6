using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Application.ResultPattern
{
    public sealed record Error(ErrorCode Code, string Message)
    {
        public static readonly Error None =
            new(ErrorCode.NoError, string.Empty);
    }
}
