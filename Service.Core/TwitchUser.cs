using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public partial class TwitchUser
    {
        [JsonProperty("_id")]
        public long Id { get; set; }

        [JsonProperty("broadcaster_language")]
        public string BroadcasterLanguage { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("followers")]
        public long Followers { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("logo")]
        public Uri Logo { get; set; }

        [JsonProperty("mature")]
        public bool Mature { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("partner")]
        public bool Partner { get; set; }

        [JsonProperty("profile_banner")]
        public Uri ProfileBanner { get; set; }

        [JsonProperty("profile_banner_background_color")]
        public object ProfileBannerBackgroundColor { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("video_banner")]
        public object VideoBanner { get; set; }

        [JsonProperty("views")]
        public long Views { get; set; }
    }
}
