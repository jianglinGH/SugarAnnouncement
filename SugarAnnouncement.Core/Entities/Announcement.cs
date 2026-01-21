using SugarAnnouncement.Core.Enums;
using SugarAnnouncement.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAnnouncement.Core.Entities
{
    public class Announcement
    {
        private string _title = string.Empty;
        private string _content = string.Empty;
        public int Id { get; set; }

        // 通过属性 setter 赋值，getter 控制访问逻辑，用私有字段保存最终值【抛错，拒绝非法值】
        public string Title { 
            get => _title; 
            set {
                if (value.Length == 0)
                {
                    throw new DomainExceptions("标题不能为空");
                }
                if (value.Length > 10) {
                    throw new DomainExceptions("标题长度不能超过10个字");
                }
                _title = value;
            } 
        } 
        
        public string Content { 
            get => _content;
            set {
                if (value.Length > 50) {
                    throw new DomainExceptions("内容长度不能超过50个字");
                }
                _content = value;
            } 
        } 
      
        public AnnouncementCategory Category { get; set; }
        // 自动实现的属性可以有默认值
        public string Author { get; set; } = string.Empty;
        public DateTime PublishTime { get; set; } // 默认值是 DateTime.MinValue，0001-01-01 00:00:00 公元1.1.1
        public bool IsTop { get; set; } = false;
    }
}
