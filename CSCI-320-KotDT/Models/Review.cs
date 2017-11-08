using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ControllerDI.Interfaces;

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

        public bool like() {
            string queryString = "UPDATE  review set like_count = like_count+1 where review_id = " + Id;
            return true;
        }
        
        public void dislike() {
            string queryString = "UPDATE  review set like_count = like_count+1 where review_id = " + Id;
        }

        private readonly IQueryHandler QueryHandler;

        public Review()
        {

        }

        public Review(int movie_id, IQueryHandler IQuery) {
            QueryHandler = IQuery;
            MovieId = movie_id;
        }
    }
}