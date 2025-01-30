using System.ComponentModel.DataAnnotations;

namespace RPGSessionManager.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "";

        [Required]
        [MaxLength(200)]
        public string About { get; set; } = "";

        [Required]
        public string HomeCity { get; set; } = "";

        //relations
        public List<Character> Characters { get; set; } = new();
        public List<Session> Sessions { get; set; } = new();
    }
}
