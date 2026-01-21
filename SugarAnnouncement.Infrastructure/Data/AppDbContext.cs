using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SugarAnnouncement.Core.Entities;

namespace SugarAnnouncement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // 对应 SQLite 数据库中的表
        public DbSet<Announcement> Announcements { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // 映射ENTITY到数据库表
        protected override void OnModelCreating(ModelBuilder modelBuilder)
       
        {
            //可选 映射表名
            modelBuilder.Entity<Announcement>()
                .ToTable("Announcements");
        }
    }
}
