using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responses.Responses
{
    public class ResponseObj<T>
    {
        public bool Success { get; set; }        /// true nếu thành công, false nếu thất bại
        public string Message { get; set; } = string.Empty;  // mô tả ngắn gọn về phản hồi
        public T? Data { get; set; }     //   Dữ liệu trả về (có thể là bất kỳ kiểu nào)
        public List<string> Errors { get; set; } // trả về kiểu lỗi
        public ResponseObj()
        {
            Errors = new List<string>();
        }
        public ResponseObj(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public ResponseObj(string message, T data)
        {
            Message = message;
            Data = data;
        }
        public static ResponseObj<T> ResponseSuccess(string message, T data)
        {
            return new ResponseObj<T>(true, message, data);
        }
    
    }
}
