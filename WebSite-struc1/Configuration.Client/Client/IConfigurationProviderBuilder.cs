using Configuration.Client.Client;
using Configuration.Client.Common;
using System;

namespace Configuration.Client
{
    public interface IConfigurationProviderBuilder
    {
        void AddDataServiceSource(string uri);

        void AddDataServiceSource(Uri uri);

        void AddAppConfigSource();

        void AddCustomConfigSource(string configFilePath, SupportedConfigFormat format);

        IRegistration NewCustomRegistration(string category, string className);

        IRegistration NewCustomRegistration(ClassName className);

        IConfigurationProvider Build();
    }
}
