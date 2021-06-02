using System;
using System.ComponentModel.DataAnnotations;

namespace Domain_inventory.Models
{
    public class CreateDomainViewModel
    {
        [Display(Name = "Domain")]
        public string name { get; set; }
        [Display(Name = "Created On")]
        public DateTime buyTime { get; set; }
        [Display(Name = "Expires On")]
        public DateTime expiresOn { get; set; }

        [Display(Name = "Safe to use?")] 
        public bool o365Cadency;

        public bool is44DaysOrOlder
        {
            get => o365Cadency = buyTime.AddDays(44) >= DateTime.Now;
            set => o365Cadency = value;
        }
    }
}