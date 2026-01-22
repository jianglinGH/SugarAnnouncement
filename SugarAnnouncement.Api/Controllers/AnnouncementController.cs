using Microsoft.AspNetCore.Mvc;
using SugarAnnouncement.Api.Common;
using SugarAnnouncement.Core.DTOs;
using SugarAnnouncement.Core.DTOs.Announcement;
using SugarAnnouncement.Core.Entities;
using SugarAnnouncement.Core.Enums;
using SugarAnnouncement.Core.Interfaces; 

namespace SugarAnnouncement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementController(IAnnouncementRepository announcementRepository) { 
            _announcementRepository = announcementRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AnnouncementQuery? query) {
            if (query == null) {
                var data = await _announcementRepository.GetAllAsync();
                return Ok(ApiResponse<List<Announcement>>.Success(data));
            }
            var result = await _announcementRepository.GetAllAsync(query?.Category, query?.PublishTime);
            return Ok(ApiResponse<List<Announcement>>.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _announcementRepository.GetByIdAsync(id);
            return Ok(ApiResponse<Announcement?>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementRequest dto)
        {
            if (!Enum.TryParse<AnnouncementCategory>(dto.Category, true, out var cat))
            {
                return BadRequest(ApiResponse<string>.Failure("分类不合法"));
            }
            // DTO -> Entity 映射
            var entity = new Announcement
            {
                Title = dto.Title,
                Content = dto.Content,
                Category = cat,
                Author = dto.Author,
                IsTop = dto.IsTop,
                PublishTime = dto.PublishTime
            };
            if(entity.PublishTime == default) {
                entity.PublishTime = DateTime.Now;
            }
            await _announcementRepository.AddAsync(entity);
            return Ok(ApiResponse<Announcement>.Success(entity));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAnnouncementRequest dto)
        {
            if (!Enum.TryParse<AnnouncementCategory>(dto.Category, true, out var cat)) { 
                return BadRequest(ApiResponse<string>.Failure("分类不合法"));
            }
            // 检查是否存在，存在更新
            var entity = await _announcementRepository.GetByIdAsync(dto.Id);
            if (entity == null) {
                return BadRequest(ApiResponse<string>.Failure("此公告不存在，无法更新"));
            }
            // DTO -> Entity 映射
            entity.Title = dto.Title;// 触发领域校验
            entity.Content = dto.Content;
            entity.Category = cat;
            entity.Author = dto.Author;
            entity.IsTop = dto.IsTop;
            entity.PublishTime = dto.PublishTime;
          
            if (entity.PublishTime == default)
            {
                entity.PublishTime = DateTime.Now;
            }
            await _announcementRepository.UpdateAsync(entity);
            return Ok(ApiResponse<Announcement>.Success(entity));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _announcementRepository.DeleteAsync(id);
            return Ok(ApiResponse<string>.Success("删除成功"));
        }
    }
}
