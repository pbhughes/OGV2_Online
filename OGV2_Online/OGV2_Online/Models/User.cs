using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OGV2_Online.Models
{
    public class User
    {
        [Required]
        public long UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        public List<Boards> BoardList { get; set; }
        
    }
}