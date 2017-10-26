using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSCI_320_KotDT.Models
{
    [Table("actors")]
    public class Actor
    {
        [Key]
        public string Name { get; set; }
        public string Performed_in { get; set; }
        public string Role { get; set; }


        public List<Tuple<Movie, string>> Filmography { get; set; }

        public Actor(string Name, string Role)
        {
            this.Name = Name;
            this.Role = Role;
        }

        public Actor(string Name)
        {
            this.Name = Name;this.Filmography = new List<Tuple<Movie, string>>();
        }

    }
}