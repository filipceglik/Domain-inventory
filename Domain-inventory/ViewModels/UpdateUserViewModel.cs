namespace Domain_inventory.Models
{
    public class UpdateUserViewModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}