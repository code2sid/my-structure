using System;

namespace Configuration.Client.Common
{
    public abstract class ConfigSourceInfo
    {
        protected ConfigSourceInfo(ConfigSourceType sourceType)
        {
            this.Id = Guid.NewGuid();
            this.SourceType = sourceType;
        }

        public Guid Id { get; }

        public ConfigSourceType SourceType { get; }
    }
}
