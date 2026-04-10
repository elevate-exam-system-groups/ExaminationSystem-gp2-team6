using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Presentation.ViewModels.Responses
{
    public class SuccessResponseViewModelT<T> : ResponseViewModel
    {
        public T Data { get; set; }
        public SuccessResponseViewModelT(T data , String message) 
        {
            IsSuccess = true;
            Data = data;
            Message = message;
        }
        public SuccessResponseViewModelT(T data ) 
        {
            IsSuccess = true;
            Data = data;
        }
    }
}
