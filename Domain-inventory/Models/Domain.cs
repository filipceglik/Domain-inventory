using System;
using System.Collections.Generic;

namespace Domain_inventory.Models
{
    public class Domain
    {
        public string name { get; set; }
        public DateTime buyTime { get; set; }
        public DateTime expiresOn { get; set; }
        public Dictionary<string,string> categories { get; set; }
    }
}