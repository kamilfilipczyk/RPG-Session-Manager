using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RPGSessionManager.Models
{
    public class Player
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //relations
        public List<Character>? Characters { get; set; }
    }
}