using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Application.ResultPattern
{
    public enum ErrorCode
    {
        NoError = 0,
        NotFound = 1,
        AlreadyExists = 2,   
        NotAvailable=3,
        InvalidData=4,
        Unauthorized =5
    }
}
