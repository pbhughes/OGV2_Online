using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ogv2_Online.Models
{
    public class User
    {
        public User()
        {
            BoardList = new List<Board>();
        }

        [Required]
        public long UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        public string SessionGuid { get; set; }

        public List<Board> BoardList { get; set; }

    }
}