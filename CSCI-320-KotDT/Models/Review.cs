using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSCI_320_KotDT.Models
{
    public class Review
    {

        public int review_id;
        public string review_text;

        [Display(Name = "Score")]
        public float score;

        [Display(Name = "Likes")]
        public int like_count;

        [Display(Name = "Dislikes")]
        public int dislike_count;

        [Display(Name = "User")]
        public string username;
        public int movie_id;
    }
}