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
        public string PerformedIn { get; set; }
        public string Role { get; set; }
        
        public List<Tuple<string, Movie>> Roles { get; set; }

        public Actor(string Name, string Role)
        {
            this.Name = Name;
            this.Role = Role;
        }

        public Actor() 
        {
        }

    }

   
}