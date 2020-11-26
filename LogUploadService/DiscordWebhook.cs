using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LogUploadService
{
    public class DiscordWebhook
    {
        public DiscordWebhook(IConfigurationRoot config) { 
            _webhookURL = config.GetSection("Discord:webhookURL").Value ?? throw new FieldAccessException("Discord:webhookURL");
            _username = config.GetSection("Discord:userName").Value ?? throw new FieldAccessException("Discord:userName");
            _avatarUrl = config.GetSection("Discord:avatarUrl").Value ?? throw new FieldAccessException("Discord:avatarUrl");
        }

        private static readonly HttpClient _client = new HttpClient();

        private string _webhookURL;
        private string _username;
        private string _avatarUrl;

        public async Task<HttpResponseMessage> SendMessageAsync(string username = null, string avatarUrl = null, string content = null)
        {
            var msg = new Message(username ?? _username, avatarUrl ?? _avatarUrl, content);
            return await _client.PostAsync(_webhookURL, new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json"));
        }

        [JsonObject]
        internal class Message
        {
            [JsonProperty("username")]
            public string Username;
            [JsonProperty("avatar_url")]
            public string AvatarUrl;
            [JsonProperty("content")]
            public string Content;

            public Message(string username, string avatarUrl, string content)
            {
                Username = username;
                AvatarUrl = avatarUrl;
                Content = content;
            }
        }
    }
}
