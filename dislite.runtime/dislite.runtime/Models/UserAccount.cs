using dislite.runtime.Parsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dislite.runtime.Models
{
    public class UserAccount
    {
        [JsonProperty]
        private string id { get; set; }
        [JsonProperty]
        private string username { get; set; }
        [JsonProperty]
        private string discriminator { get; set; }

        [JsonIgnore]
        public string Username => username;

        [JsonIgnore]
        public int Discriminator => int.Parse(discriminator);

        [JsonIgnore]
        public Snowflake Id => Snowflake.Parse(id);

        [JsonIgnore]
        public string AccessToken { get; set; }

        public override string ToString()
        {
            return $"{id}: {Username} ({discriminator})";
        }
    }
}
