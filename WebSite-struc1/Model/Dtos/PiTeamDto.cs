using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dtos
{
    public class PiTeamDto
    {
        public PiTeamDto(int refId, string name, bool isAttached)
        {
            RefId = refId;
            Name = name;
            IsAttached = isAttached;
        }

        public int RefId { get; set; }
        public string Name { get; set; }
        public bool IsAttached { get; set; }
    }
}
