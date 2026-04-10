using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Presentation.ViewModels.Responses
{
    public class FailedResponseViewModel : ResponseViewModel
    {
        public ErrorCode ErrorType { get; set; }

        public FailedResponseViewModel(ErrorCode errorType, string message) 
        {
            Message = message;
            IsSuccess = false;
            ErrorType = errorType;
        }
    }
}
