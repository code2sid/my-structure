using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Currency
    {
        public Currency(string id, string name)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; }
        public string Id { get; }
    }
}
