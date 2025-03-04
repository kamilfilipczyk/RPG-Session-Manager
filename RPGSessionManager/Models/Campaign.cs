﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RPGSessionManager.Models
{
    public class Campaign
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string About { get; set; }

        public bool HasEnded { get; set; }

        //relations
        public List<Session>? Sessions { get; set; }
    }
}