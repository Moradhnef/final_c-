using System.ComponentModel.DataAnnotations;

namespace Server.EntityFramework.Entities
{
    public class BaseObject
    {
        [Key]
        public int Id { get; set; }
    }
}
