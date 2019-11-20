using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class CosmosSettings
    {
        public string AuthKey { get; set; }
        public string EndpointUrl { get; set; }
        public string DatabaseId { get; set; }
        public string ContainerId { get; set; }
    }
}
