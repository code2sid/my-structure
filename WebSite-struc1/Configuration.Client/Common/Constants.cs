using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Common
{
    public static class Constants
    {
        public static readonly StringComparer StringComparer = StringComparer.OrdinalIgnoreCase;
        public const int DefaultTimeoutSeconds = 300;
        public const string XCorrelationIdHeader = "x-correlation-id";
    }
}
