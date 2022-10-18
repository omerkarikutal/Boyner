using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public BaseResponse<T> Success(T data)
        {
            return new BaseResponse<T> { Status = true, Data = data };
        }

        public BaseResponse<T> Fail(string message)
        {
            return new BaseResponse<T> { Status = false, ErrorMessage = message };
        }
    }
}
