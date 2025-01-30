using System.ComponentModel.DataAnnotations;

namespace RPGSessionManager.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        //relations
        public List<Character> Characters { get; set; } = new();
    }
}
