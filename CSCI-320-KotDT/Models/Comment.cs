using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSCI_320_KotDT.Models
{
    public class Comment
    {

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Posted { get; set; }

        [Required]
        [Display(Name = "User")]
        public string CreatedBy { get; set; }

        [Required]
        public int ParentID { get; set; }

        public Comment()
        {
        }

        public Comment(DateTime posted)
        {
            Posted = posted;
        }

        public Comment(int parentID)
        {
            ParentID = parentID;
        }
    }
}