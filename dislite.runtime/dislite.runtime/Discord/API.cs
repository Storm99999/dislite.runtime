using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using dislite.runtime.Parsers;
using dislite.runtime.Models;
using System.Windows.Forms;
using dislite.runtime.Setup;

namespace dislite.runtime.Discord
{
    public static class API
    {
        private static HttpClient _client = new HttpClient();
        private static string ApiVersion = "v9";

        public static async Task<List<Guild>> ListGuilds()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://discord.com/api/{ApiVersion}/users/@me/guilds");
            request.Method = HttpMethod.Get;
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(Settings.Default.token);

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Guild>>(json);
        }

        public static async Task<UserAccount> GetUser()
        {
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri($"https://discord.com/api/{ApiVersion}/users/@me");
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(Settings.Default.token);

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Response code errored. Your token is most likely invalid. ");
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserAccount>(json);
        }

        public static async Task<List<Channel>> ListChannels(Guild guild)
        {
            return await ListChannels(guild.Id.ToString());
        }

        public static async Task SendMessage(string content, string ChannelId)
        {
            if (ChannelId is null)
            {
                ChannelId = "1188171362641125389";
            }

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://discord.com/api/{ApiVersion}/channels/{ChannelId}/messages"); // channel msg endpoint
            request.Method = HttpMethod.Post;
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(Settings.Default.token);

            var nonce = Snowflake.NewSnowflake().ToString();
            var body = new
            {
                content,
                nonce,
                tts = false
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<List<Channel>> ListChannels(string guildId)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://discord.com/api/{ApiVersion}/guilds/{guildId}/channels");
            request.Method = HttpMethod.Get;
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(Settings.Default.token);

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Channel>>(json);
        }

        public static async Task<List<DiscordMessage>> FetchMessages(string ChannelId, int limit)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://discord.com/api/{ApiVersion}/channels/{ChannelId}/messages?limit={limit}");
            request.Method = HttpMethod.Get;
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(Settings.Default.token);

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var messages = JsonConvert.DeserializeObject<List<DiscordMessage>>(json);

            foreach (var item in messages)
            {
                item.Content = item.Content.Replace("\r\n", "\\n")
                            .Replace("\n", "\\n")
                            .Replace("```", "")  // Remove triple backticks because of codeblocks
                            .Replace("`", "");    // Remove single backticks because of single line code blocks

                item.Timestamp = DateTime.Parse(item.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                item.Author.Avatar = await GetAvatarUrlAsync(item.Author.Id.ToString());
            }

            return messages;
        }

        // had to do this one differently
        public static async Task<string> GetAvatarUrlAsync(string userId)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://discord.com/api/{ApiVersion}/users/{userId}");
            request.Method = HttpMethod.Get;

            request.Headers.Authorization = AuthenticationHeaderValue.Parse(Settings.Default.token);

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<FakeUser>(json);
                // fucking avatar hash, rot in hell.
                return $"https://cdn.discordapp.com/avatars/{userId}/{user.Avatar}.png";
            }

            // Handle errors?
            return "eaea";
        }

    }
}

