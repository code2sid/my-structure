using Common.Logging;
using Common2.Interfaces;
using System;

namespace Common2
{
    public class Logger<T> : ILoggingService<T>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(T));
        public void Debug(string message) => _logger.Debug(message);

        public void DebugFormat(string template, params object[] args) => _logger.DebugFormat(template, args);

        public void Info(string message) => _logger.Info(message);

        public void InfoFormat(string template, params object[] args) => _logger.InfoFormat(template, args);

        public void Warn(string message) => _logger.Warn(message);

        public void WarnFormat(string template, params object[] args) => _logger.WarnFormat(template, args);

        public void Error(string message) => _logger.Error(message);

        public void Error(string message, Exception exception) => _logger.Error(message, exception);

        public void ErrorFormat(string template, params object[] args) => _logger.ErrorFormat(template, args);

        public void ErrorFormat(string template, Exception exception, params object[] args) => _logger.ErrorFormat(template, exception, args);
    }
}
