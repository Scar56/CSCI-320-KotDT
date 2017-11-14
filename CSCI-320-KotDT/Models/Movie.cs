using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSCI_320_KotDT.Models
{
    [Table("movies")]
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }

        [Display(Name = "Release Year")]
        public int ReleaseYear { get; set; }
        [Display(Name = "Running Time")]
        public int RunningTime { get; set; }

        public string[] Genres { get; set; }

        public string Director { get; set; }

        public Review[] Reviews { get; set; }

        public Review NewReview;

        [Display(Name = "Cast")]
        public List<Actor> Cast { get; set; }

        public Movie(string Title)
        {
            this.Title = Title;
        }

        public Movie(string Title, int Release_year, int Running_time, int id)
        {
            this.Title = Title;
            this.ReleaseYear = Release_year;
            this.RunningTime = Running_time;
            this.MovieId = id;
        }

        public Movie(string Title, int id)
        {
            this.Title = Title;
            this.MovieId = id;
        }

        public static string OrderingString()
        {
            return " order by(select score from moviescore where movies.id = moviescore.id), title ";
        }

    }
}

