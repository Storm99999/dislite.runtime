using System;

namespace dislite.runtime.Parsers
{
    public class Snowflake
    {
        private ulong _value = 0ul;


        public const ulong DiscordEpoch = 1420070400000;

        public override string ToString()
        {
            return _value.ToString();
        }

        public static Snowflake Parse(string str)
        {
            var snowflake = new Snowflake();
            snowflake._value = ulong.Parse(str);
            return snowflake;
        }

        private static ulong id = 0ul;
        private static readonly object idLock = new object();

        public static Snowflake NewSnowflake()
        {
            var snowFlake = new Snowflake();
            var time = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            time -= DiscordEpoch;
            snowFlake._value |= time << 22;

            // so we don't get a retarded error.
            lock (idLock)
            {
                id = (id + 1) & 0xFFF;
                snowFlake._value |= id;
            }

            return snowFlake;
        }

    }
}
