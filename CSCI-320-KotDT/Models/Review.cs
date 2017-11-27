﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSCI_320_KotDT.Models
{
    public class Review
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Review")]
        public string ReviewText { get; set; }

        [Required]
        [Display(Name = "Score")]
        public float Score { get; set; }

        [Display(Name = "Likes")]
        public int LikeCount { get; set; }

        [Display(Name = "Dislikes")]
        public int DislikeCount { get; set; }

        [Required]
        [Display(Name = "User")]
        public string CreatedBy { get; set; }

        public int MovieId { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<Comment> Comments { get; set; }

        public Review(int movie_id)
        {
            this.MovieId = movie_id;
        }

        public Review()
        {

        }
    }
}