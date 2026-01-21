using SugarAnnouncement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAnnouncement.Core.DTOs.Announcement
{
    public class AnnouncementResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public AnnouncementCategory Category { get; set; }
        public bool IsTop { get; set; }
        public DateTime PublishTime { get; set; }
        public string Author { get; set; } = string.Empty;
    }
}
