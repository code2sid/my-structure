using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common2.Interfaces
{
    public interface ILoggingService<TLoggingTarget>
    {
        void Debug(string message);

        void DebugFormat(string template, params object[] args);

        void Info(string message);

        void InfoFormat(string template, params object[] args);

        void Warn(string message);

        void WarnFormat(string template, params object[] args);

        void Error(string message);

        void Error(string message, Exception exception);

        void ErrorFormat(string template, params object[] args);

        void ErrorFormat(string template, Exception exception, params object[] args);
    }
}
