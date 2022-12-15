using NLog;
using NLog.Web;

namespace Covid.Helper
{
    public static class LogHelper
    {
        private static Logger log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public static void Info(string message)
        {
            log.Info(message);
        }

        public static void Debug(string message)
        {
            log.Debug(message);
        }

        public static void Error(string message)
        {
            log.Error(message);
        }
    }
}