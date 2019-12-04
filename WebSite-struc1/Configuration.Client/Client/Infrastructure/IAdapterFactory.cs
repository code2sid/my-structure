using Configuration.Client.Common;
using System.Collections.Generic;

namespace Configuration.Client.Client.Infrastructure
{
    public interface IAdapterFactory
    {
        IAdapter Create(ConfigSourceInfo info);

        IAdapter Create(IReadOnlyDictionary<ClassName, ClassRegistration> customRegistrations);
    }
}
