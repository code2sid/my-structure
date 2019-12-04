using Configuration.Client.Client.Infrastructure;
using Configuration.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Configuration.Client
{
    public sealed class ConfigurationProviderBuilder : IConfigurationProviderBuilder
    {
        private readonly ConfigurationBuilderContext _context;
        private readonly IAdapterFactory _adapterFactory;
        private readonly ITimeoutInvalidator _invalidator;

        public ConfigurationProviderBuilder(string application)
          : this(new EnvironmentInfo(application), (ITimeoutInvalidator)new TimeoutInvalidator())
        {
        }

        public ConfigurationProviderBuilder(string application, string environment)
          : this(new EnvironmentInfo(application, environment), (ITimeoutInvalidator)new TimeoutInvalidator())
        {
        }

        internal ConfigurationProviderBuilder(EnvironmentInfo environment, ITimeoutInvalidator invalidator)
          : this(new ConfigurationBuilderContext(environment), (IAdapterFactory)new AdapterFactory(environment, (IConfigLoaderFactory)new ConfigLoaderFactory(), (IDomValidator)new DomValidator()), invalidator)
        {
        }

        internal ConfigurationProviderBuilder(ConfigurationBuilderContext context, IAdapterFactory adapterFactory, ITimeoutInvalidator invalidator)
        {
            this._context = context;
            this._adapterFactory = adapterFactory;
            this._invalidator = invalidator;
        }

        public void AddDataServiceSource(string uri)
        {
            this.AddDataServiceSourcePrivate(new Uri(uri.CheckNotNull<string>(nameof(uri))));
        }

        public void AddDataServiceSource(Uri uri)
        {
            this.AddDataServiceSourcePrivate(uri.CheckNotNull<Uri>(nameof(uri)));
        }

        public void AddAppConfigSource()
        {
            this.CheckAppConfigSourceUnique();
            this._context.Sources.Add((ConfigSourceInfo)new AppConfigSource());
        }

        public void AddCustomConfigSource(string configFilePath, SupportedConfigFormat format)
        {
            configFilePath.CheckNotNull<string>(nameof(configFilePath));
            this.CheckCustomConfigSourceUnique(configFilePath);
            this._context.Sources.Add((ConfigSourceInfo)new LocalConfigSource(configFilePath, format));
        }

        public IRegistration NewCustomRegistration(string category, string className)
        {
            return this.NewCustomRegistration(new ClassName(category, className));
        }

        public IRegistration NewCustomRegistration(ClassName className)
        {
            this.CheckCustomRegistrationUnique(className);
            return (IRegistration)new Registration(className, this._context);
        }

        public IConfigurationProvider Build()
        {
            return (IConfigurationProvider)new ConfigurationProvider(new Func<IAdapterPipeline>(this.CreatePipeline), this._invalidator);
        }

        private IAdapterPipeline CreatePipeline()
        {
            List<IAdapter> adapterList = new List<IAdapter>();
            if (this._context.CustomRegistrations.Any<KeyValuePair<ClassName, ClassRegistration>>())
                adapterList.Add(this._adapterFactory.Create((IReadOnlyDictionary<ClassName, ClassRegistration>)this._context.CustomRegistrations));
            adapterList.AddRange(this._context.Sources.Select<ConfigSourceInfo, IAdapter>((Func<ConfigSourceInfo, IAdapter>)(s => this._adapterFactory.Create(s))));
            return (IAdapterPipeline)new AdapterPipeline((IEnumerable<IAdapter>)adapterList);
        }

        private void AddDataServiceSourcePrivate(Uri uri)
        {
            this.CheckDataServiceUriUnique(uri);
            this._context.Sources.Add((ConfigSourceInfo)new DataServiceConfigSource(uri));
        }

        private void CheckDataServiceUriUnique(Uri uri)
        {
            string str = uri.ToString();
            if (this._context.Sources.OfType<DataServiceConfigSource>().Any<DataServiceConfigSource>((Func<DataServiceConfigSource, bool>)(s => s.Uri.ToString().Equals(str, StringComparison.OrdinalIgnoreCase))))
                throw new ArgumentException(string.Format("Service source with base address [{0}] already registered", (object)str));
        }

        private void CheckAppConfigSourceUnique()
        {
            if (this._context.Sources.OfType<AppConfigSource>().Any<AppConfigSource>())
                throw new ArgumentException("App configuration source already registered");
        }

        private void CheckCustomConfigSourceUnique(string filePath)
        {
            if (this._context.Sources.OfType<LocalConfigSource>().Any<LocalConfigSource>((Func<LocalConfigSource, bool>)(s => s.ConfigFilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase))))
                throw new ArgumentException(string.Format("Custom config source with file path [{0}] already registered", (object)filePath));
        }

        private void CheckCustomRegistrationUnique(ClassName name)
        {
            if (this._context.CustomRegistrations.ContainsKey(name))
                throw new ArgumentException(string.Format("Custom registration with id: [{0}] already exists", (object)name));
        }
    }
}
