using Configuration.Client.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Common
{
    public sealed class ClassName : IEquatable<ClassName>, ICloneable
    {
        public static readonly ClassName Empty = new ClassName(string.Empty, string.Empty);

        public ClassName(string category, string name)
        {
            this.Category = category.CheckNotNull<string>(nameof(category));
            this.Name = name.CheckNotNull<string>(nameof(name));
        }

        public string Category { get; }

        public string Name { get; }

        public bool Equals(ClassName other)
        {
            if ((object)this == (object)other)
                return true;
            if ((object)other != null && Constants.StringComparer.Equals(this.Category, other.Category))
                return Constants.StringComparer.Equals(this.Name, other.Name);
            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ClassName);
        }

        public override int GetHashCode()
        {
            return Constants.StringComparer.GetHashCode(this.Category) * 397 ^ Constants.StringComparer.GetHashCode(this.Name);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", (object)this.Category, (object)this.Name);
        }

        public object Clone()
        {
            return (object)new ClassName(this.Category, this.Name);
        }

        public static bool operator ==(ClassName left, ClassName right)
        {
            return object.Equals((object)left, (object)right);
        }

        public static bool operator !=(ClassName left, ClassName right)
        {
            return !object.Equals((object)left, (object)right);
        }

        public static ClassName Parse(string stringified)
        {
            string[] strArray = stringified.Split(':');
            if (strArray.Length != 2)
                throw new FormatException("Invalid settings class name. Should be \"<category>:<name>\"");
            return new ClassName(strArray[0], strArray[1]);
        }

        public static implicit operator ClassName(string strValue)
        {
            return ClassName.Parse(strValue);
        }
    }
}
