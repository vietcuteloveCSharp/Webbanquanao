using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Extensions
{
    public static class CookieExtensions
    {
        // Lưu một đối tượng dưới dạng JSON vào cookie
        public static void SetObjectAsJson(this IResponseCookies cookies, string key, object value, CookieOptions options)
        {
            cookies.Append(key, JsonConvert.SerializeObject(value), options);
        }

        // Lấy một đối tượng từ cookie và chuyển đổi từ JSON về đối tượng
        public static T GetObjectFromJson<T>(this IRequestCookieCollection cookies, string key)
        {
            var value = cookies[key];
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
