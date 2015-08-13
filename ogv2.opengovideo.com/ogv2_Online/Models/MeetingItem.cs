using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ogv2_Online.Models
{
    public class MeetingItem
    {
        [Required]
        public int ItemID { get; set; }

        public int ParentID { get; set; }

        [Required]
        public int MeetingID { get; set; }

        public int SortNumber { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public int TimeStamp { get; set; }

        public List<MeetingItem> Items { get; set; }
    }
}