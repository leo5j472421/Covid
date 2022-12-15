using Covid.Enums;
using Covid.Helper;

namespace Covid.Model
{
    public class ApiBaseResponse<T>
    {
        public EnumError ErrorCode { get; set; }
        public T Data { get; set; }
        public ApiBaseResponse() {  }

        public ApiBaseResponse(EnumError errorCode)
        {
            ErrorCode = errorCode;
        }
        public ApiBaseResponse(EnumError errorCode, T data)
        {
            ErrorCode = errorCode;
            Data = data;
        }

        public string ErrorMessage => ErrorCode.Description();

        public bool ShouldSerializeData()
        {
            return Data != null;
        }
    }
}