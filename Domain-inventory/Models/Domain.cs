using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain_inventory.Models
{
    public class Domain
    {
        [Display(Name = "Domain")]
        public string name { get; set; }
        [Display(Name = "Created On")]
        public DateTime buyTime { get; set; }
        [Display(Name = "Expires On")]
        public DateTime expiresOn { get; set; }
        [Display(Name = "Safe to use?")]
        public bool o365Cadency { get; set; }
        //public Dictionary<string,string> categories { get; set; }
        
        public virtual ICollection<Domain> Domains { get; set; }
    }
}