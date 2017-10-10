using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CSCI_320_KotDT.Models
{
    [Table("movies")]
    public class Movie
    {
        [Key]
        public int MovieId{ get; set; }

        public string Title { get; set; }

        [Display(Name = "Release Year")]
        public int Release_year { get; set; }
        public int Running_time { get; set; }

        public Movie(string Title)
        {
            this.Title = Title;
        }

        public Movie(string Title, int Release_year, int Running_time)
        {
            this.Title = Title;
            this.Release_year = Release_year;
            this.Running_time = Running_time;
        }
        

    }

    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
    }
}