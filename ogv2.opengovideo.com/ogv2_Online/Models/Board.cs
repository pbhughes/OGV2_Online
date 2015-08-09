using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ogv2_Online.Models
{
    public class Board
    {

        [Required]
        public long BoardID { get; set; }

        [Required]
        public long UserID { get; set; }

        [Required]
        public string BoardName { get; set; }


    }
}