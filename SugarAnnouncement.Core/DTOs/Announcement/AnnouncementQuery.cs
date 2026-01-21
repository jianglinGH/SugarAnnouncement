using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAnnouncement.Core.DTOs.Announcement
{
    public class AnnouncementQuery
    {
        public string? Category { get; set; } 
        public DateTime? PublishTime { get; set; } 
    }
}
