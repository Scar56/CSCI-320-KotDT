using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSCI_320_KotDT.Models
{
    [Table("movies")]
    public class Movie
    {
        [Key]
<<<<<<< Updated upstream
        public int MovieId { get; set; }
=======
        public int MovieId{ get; set; }
>>>>>>> Stashed changes

        public string Title { get; set; }

        [Display(Name = "Release Year")]
        public int Release_year { get; set; }
<<<<<<< Updated upstream

        [Display(Name = "Running Time")]
=======
>>>>>>> Stashed changes
        public int Running_time { get; set; }

        public Movie(string Title)
        {
            this.Title = Title;
        }

        public Movie(string Title, int Release_year, int Running_time, int id)
        {
            this.Title = Title;
            this.Release_year = Release_year;
            this.Running_time = Running_time;
            this.MovieId = id;
        }
<<<<<<< Updated upstream


    }
}



=======
        

    }
}
>>>>>>> Stashed changes
