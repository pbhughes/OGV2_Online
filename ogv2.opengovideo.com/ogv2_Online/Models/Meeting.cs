using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ogv2_Online.Models
{
    public class Meeting
    {
        [Required]
        public int MeetingID { get; set; }

        [Required]
        public int BoardID { get; set; }

        [Required]
        public string MeetingName { get; set; }

        [Required]
        public DateTime MeetingDate { get; set; }

        public string RecordingURL { get; set; }

        public List<MeetingItem> Items { get; set; }


    }
}