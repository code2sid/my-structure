using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Country
    {
        public Country(string name, string id, IEnumerable<Currency> currencies)
        {
            Name = name;
            Id = id;
            Currencies = currencies;
        }

        public string Name { get; }
        public string Id { get; }
        public IEnumerable<Currency> Currencies { get; }
    }
}
