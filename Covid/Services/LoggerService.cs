using System;
using System.Runtime.CompilerServices;
using Covid.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Covid.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger) => _logger = logger;

        public void Debug(string message, [CallerMemberName] string caller = "")
        {
            _logger.LogDebug($"{caller}() - {message}");
        }

        public void Info(string message)
        {
            _logger.LogInformation($"{message}");
        }

        public void Error(string message, Exception exception = null, string caller = "")
        {
            if (exception == null)
            {
                _logger.LogError($"{caller}() - {message}");
            }
            else
            {
                _logger.LogError(exception, $"{caller}() - {message}");
            }
        }

        public void Error(string message, [CallerMemberName] string caller = "")
        {
            _logger.LogError($"{caller}() - {message}");
        }
    }
}