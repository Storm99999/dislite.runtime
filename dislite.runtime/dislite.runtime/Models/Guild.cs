using dislite.runtime.Parsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dislite.runtime.Models
{
    public class Guild
    {
        [JsonProperty]
        private string id { get; set; }
        [JsonProperty]
        private string name { get; set; }

        [JsonIgnore]
        public string Name => name;

        [JsonIgnore]
        public Snowflake Id => Snowflake.Parse(id);

        public override string ToString()
        {
            return $"{id}: {Name}";
        }
    }
}
