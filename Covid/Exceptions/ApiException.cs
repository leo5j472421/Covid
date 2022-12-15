using System;
using Covid.Enums;

namespace Covid.Exceptions
{
    public class ApiException : Exception
    {
        public EnumError Error { get; set; }
        public ApiException(string message , EnumError errorCode) : base ( message )
        {
            Error = errorCode;
        }
    }
}