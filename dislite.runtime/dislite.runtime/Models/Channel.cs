using dislite.runtime.Parsers;
using Newtonsoft.Json;

namespace dislite.runtime.Models
{
    public enum ChannelType
    {
        GuildText = 0,
        Dm = 1,
        GuildVoice = 2,
        GroupDm = 3,
        GuildCategory = 4,
        GuildNews = 5,
        GuildStore = 6,
        GuildNewsThread = 10,
        GuildPublicThread = 11,
        GuildPrivateThread = 12,
        GuildStageVoice = 13
    }

    public class Channel
    {
        [JsonProperty]
        private string id { get; set; }
        [JsonProperty]
        private int type { get; set; }
        [JsonProperty]
        private string name { get; set; }

        [JsonIgnore]
        public ChannelType Type => (ChannelType)type;

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
