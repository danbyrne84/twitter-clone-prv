using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterClone.Api.Config
{
    public class CosmosDBSettings
    {
        public string Endpoint { get; set; }
        public string AuthKey { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
