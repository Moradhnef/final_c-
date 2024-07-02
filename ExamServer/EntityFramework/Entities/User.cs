using Azure.Identity;

namespace Server.EntityFramework.Entities
{
    public class User : BaseObject
    { 
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLector { get; set; }

    }
}
