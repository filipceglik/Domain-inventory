using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain_inventory.Models
{
    public class Domain
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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