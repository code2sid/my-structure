using Configuration.Client.Common;
using System;

namespace Configuration.Client.Client.Infrastructure
{
    public interface IAdapter : IAdapterPipeline, IDisposable
    {
        ConfigSourceInfo TargetSource { get; }
    }
}
