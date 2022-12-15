using System;
using System.Runtime.CompilerServices;

namespace Covid.Services.Interfaces
{
    public interface ILoggerService
    {
        void Debug(string message, [CallerMemberName] string caller = "");

        void Info(string message);

        void Error(string message, Exception exception = null, [CallerMemberName] string caller = "");
    }
}