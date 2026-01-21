using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SugarAnnouncement.Core.Entities;

namespace SugarAnnouncement.Core.Interfaces
{
    // abstract 用于修饰类或者方法，interface 本身就是抽象类型
    public interface IAnnouncementRepository
    {
        Task<List<Announcement>> GetAllAsync();
        Task<List<Announcement>> GetAllAsync(string? category, DateTime? publishTime); 
        Task<Announcement?> GetByIdAsync(int id);
        Task AddAsync(Announcement entity);
        Task UpdateAsync(Announcement entity);
        Task DeleteAsync(int id);
        
    }
}
