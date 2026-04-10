using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Presentation.ViewModels.Responses
{
    public class SuccessResponseViewModel: ResponseViewModel
    {
        public SuccessResponseViewModel(string message) 
        {
            IsSuccess = true;
            Message = message;
        }
        public SuccessResponseViewModel() 
        {
            IsSuccess = true;
        }
    }
}
