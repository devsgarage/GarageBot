using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class StreamData
    {
        public string Type { get; set; }

        public string Title { get; set; }

        [JsonProperty(PropertyName ="viewer_count")]
        public int ViewerCount { get; set; }

        [JsonProperty(PropertyName = "started_at")]
        public DateTime StartedAt { get; set; }

        public string Language { get; set; }
    }
}
