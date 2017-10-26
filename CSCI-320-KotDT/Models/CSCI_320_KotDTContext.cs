using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CSCI_320_KotDT.Models
{
    public class CSCI_320_KotDTContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public CSCI_320_KotDTContext() : base("name=CSCI_320_KotDTContext")
        {
        }

        public System.Data.Entity.DbSet<CSCI_320_KotDT.Models.Actor> Actors { get; set; }
    }
}
