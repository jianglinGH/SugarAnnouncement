using Microsoft.EntityFrameworkCore;
using SugarAnnouncement.Core.Entities;
using SugarAnnouncement.Core.Enums;
using SugarAnnouncement.Core.Exceptions;
using SugarAnnouncement.Core.Interfaces;
using SugarAnnouncement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace SugarAnnouncement.Infrastructure.Respositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AppDbContext _db;
        public AnnouncementRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Announcement>> GetAllAsync()
        {
            return await _db.Announcements
                .OrderByDescending(a => a.IsTop)
                .ThenByDescending(a => a.PublishTime)
                .ToListAsync();
        }

        public async Task<List<Announcement>> GetAllAsync(string? category, DateTime? publishTime)
        {
            var q = _db.Announcements.AsQueryable(); 
            if (!string.IsNullOrEmpty(category)) {
                if (Enum.TryParse<AnnouncementCategory>(category, true, out var cat))
                {
                    q = q.Where(a => a.Category == cat);
                }
                else {
                    return new List<Announcement>();
                }
              
            }
            if (publishTime.HasValue)
            {
                q = q.Where(a => a.PublishTime.Date == publishTime.Value.Date);
            }
            
            q = q.OrderByDescending(a => a.IsTop)
                .ThenByDescending(a => a.PublishTime);
            return await q.ToListAsync();
        }


        public async Task<Announcement?> GetByIdAsync(int id)
        {
            return await _db.Announcements.FindAsync(id);
        }

        public async Task AddAsync(Announcement entity)
        {
            await _db.Announcements.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Announcement entity)
        { 
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Announcements.FindAsync(id);
            if (entity != null)
            {
                _db.Announcements.Remove(entity);
                await _db.SaveChangesAsync();
            }
            else {
                throw new DomainExceptions("公告不存在，无法删除");
            }
        }
    }
}