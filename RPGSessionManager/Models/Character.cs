using System.ComponentModel.DataAnnotations;

namespace RPGSessionManager.Models
{
    public class Character
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        [Required]
        public string Profession { get; set; } = "";

        public int TeamId { get; set; }

        public int PlayerId { get; set; }

        //relations
        public Team Team { get; set; }
        public Player Player { get; set; }
    }
}
