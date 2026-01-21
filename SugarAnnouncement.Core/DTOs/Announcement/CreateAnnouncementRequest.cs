using SugarAnnouncement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAnnouncement.Core.DTOs.Announcement
{
    public class CreateAnnouncementRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public bool IsTop { get; set; }
        public DateTime PublishTime { get; set; }

    }
}
