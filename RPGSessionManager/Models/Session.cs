using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RPGSessionManager.Models
{
    public class Session
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string About { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        public int? CampaignId { get; set; }

        public int TeamId { get; set; }

        //relations
        public Campaign? Campaign { get; set; }
        public Team? Team { get; set; }
    }
}