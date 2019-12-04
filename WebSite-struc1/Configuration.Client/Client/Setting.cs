using Configuration.Client.Common;
using Configuration.Client.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client
{
    public sealed class Setting : IEquatable<Setting>, ICloneable
    {
        public Setting(string key, string value)
        {
            this.Key = key.CheckNotNull<string>(nameof(key));
            this.Value = value.CheckNotNull<string>(nameof(value));
        }

        public string Key { get; }

        public string Value { get; }

        public object ValueAs(Type valueType)
        {
            return this.Value.ConvertTo(valueType);
        }

        public TValue ValueAs<TValue>()
        {
            return (TValue)this.ValueAs(typeof(TValue));
        }

        public bool Equals(Setting other)
        {
            if ((object)this == (object)other)
                return true;
            if ((object)other != null && Constants.StringComparer.Equals(this.Key, other.Key))
                return string.Equals(this.Value, other.Value);
            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Setting);
        }

        public override int GetHashCode()
        {
            return Constants.StringComparer.GetHashCode(this.Key) * 397 ^ this.Value.GetHashCode();
        }

        public object Clone()
        {
            return (object)new Setting(this.Key, this.Value);
        }

        public static bool operator ==(Setting left, Setting right)
        {
            return object.Equals((object)left, (object)right);
        }

        public static bool operator !=(Setting left, Setting right)
        {
            return !object.Equals((object)left, (object)right);
        }
    }
}
