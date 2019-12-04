using Configuration.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Client.Infrastructure
{
    public sealed class FindClassResult
    {
        public FindClassResult()
          : this(Guid.Empty, (ClassName)null)
        {
        }

        public FindClassResult(Guid configSourceId, ClassName parent)
        {
            this.ConfigSourceId = configSourceId;
            this.Parent = parent;
        }

        public bool IsSuccess
        {
            get
            {
                return this.ConfigSourceId != Guid.Empty;
            }
        }

        public Guid ConfigSourceId { get; }

        public ClassName Parent { get; }
    }
}
