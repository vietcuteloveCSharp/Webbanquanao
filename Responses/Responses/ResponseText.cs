using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responses.Responses
{
    public class ResponseText
    {
        public bool Success { get; set; } // kiểm tra xem nó thành công không
        public string Message { get; set; } = string.Empty;   // Thông báo cho người dùng
        public string Token { get; set; }
        public int Id_ChucVu { get; set; } // Thêm thuộc tính này để chứa ID chức vụ
        public ResponseText()
        {

        }
        public ResponseText(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public ResponseText(string message)
        {
            Message = message;
        }
        public ResponseText(bool success, string message, string token, int id_ChucVu)
        {
            Success = success;
            Message = message;
            Token = token;
            Id_ChucVu = id_ChucVu;
        }
        public static ResponseText ResponseSuccess(bool succes, string message, string token, int idChucVu)
        {
            return new ResponseText(succes, message, token, idChucVu);
        }
    }
}
