using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace billGenerator.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientGST { get; set; }
        public string ClientPhone { get; set; }
        public string ClientPincode { get; set; }
    }
}
