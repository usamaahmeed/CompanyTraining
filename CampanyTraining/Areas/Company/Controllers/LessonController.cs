using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace CompanyTraining.Areas.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly Cloudinary _cloudinary;
        private readonly IModuleRepository _moduleRepository;

        public LessonController(
            ILessonRepository lessonRepository,
            Cloudinary cloudinary,
            IModuleRepository moduleRepository
            )
        {
            this._lessonRepository = lessonRepository;
            this._cloudinary = cloudinary;
            this._moduleRepository = moduleRepository;
        }
        private string? GetPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);

            var parts = uri.AbsolutePath.Split('/');

            int uploadIndex = Array.IndexOf(parts, "upload");

            if (uploadIndex == -1 || (uploadIndex + 2) > parts.Length)
                return null;

            var publicIdPart = parts.Skip(uploadIndex + 2);
            string fullPath = string.Join("/", publicIdPart);
            var publicId = Path.ChangeExtension(fullPath, null);

            return publicId;
        }
        private async Task<string> UploadVideoAsync(IFormFile video, CancellationToken cancellationToken = default)
        {
            await using var stream = video.OpenReadStream();
            {
                var uploadParams = new VideoUploadParams()
                {
                    UniqueFilename = true,
                    File = new FileDescription(video.FileName, stream),
                    PublicId = Guid.NewGuid().ToString(),
                    Folder = "CompanyTraining",
                    EagerAsync = true,
                    EagerTransforms = new List<Transformation>
                 {
                      new EagerTransformation().Width(300).Height(300).Crop("pad").AudioCodec("none"),
                      new EagerTransformation().Width(160).Height(100).Crop("crop").Gravity("south").AudioCodec("none")
                 },

                };

                var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
                return result.SecureUrl.ToString();
            }
        }

        [HttpPost("/api/modules/{moduleId}/Lessons")]
        public async Task<IActionResult> Create([FromRoute] int moduleId, [FromForm] LessonRequest lessonRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var module = _moduleRepository.GetOne(e => e.Id == moduleId && e.Course.ApplicationUserId == userId, includes: [
                e=>e.Course,
                ]);

            if (module == null) return Forbid();

            var lesson = lessonRequest.Adapt<Lesson>();
            lesson.ModuleId = moduleId;
            try
            {
                lesson.VideoUrl = await UploadVideoAsync(lessonRequest.Video);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
            await _lessonRepository.CreateAsync(lesson);

            return Ok(new
            {
                Message = "Lesson Created Successfully",
                Success = true,
                Data = lesson.Adapt<LessonResponse>()
            });
        }

        [HttpGet("/api/modules/{moduleId}/Lessons")]
        public IActionResult GetAll([FromRoute] int moduleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var module = _moduleRepository.GetOne(expression: e => e.Id == moduleId && e.Course.ApplicationUserId == userId, includes: [
                e=>e.Course,
                ]);
            if (module == null) return NotFound();

            var lessons = _lessonRepository.Get(expression: e => e.ModuleId == module.Id);
            if (lessons == null) return NotFound();
            return Ok(new
            {
                Message = "Get All Lessons Successfully",
                Success = true,
                Data = lessons.Adapt<IEnumerable<LessonResponse>>()
            });
        }

        [HttpDelete("/api/modules/{moduleId}/Delete/Lessons/{lessonId}")]
        public async Task<IActionResult> Delete([FromRoute] int lessonId, [FromRoute] int moduleId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var lesson = _lessonRepository.GetOne(expression: e => e.Id == lessonId && e.ModuleId == moduleId && e.Module.Course.ApplicationUserId == userId, includes: [
                 e=>e.Module.Course,
                ]);
            if (lesson == null) return NotFound();

            string lessonUrl = GetPublicIdFromUrl(lesson.VideoUrl);

            if (lessonUrl == null) return NotFound();

            await _cloudinary.DestroyAsync(new DeletionParams(lessonUrl) { ResourceType = ResourceType.Video });


            await _lessonRepository.DeleteAsync(lesson);
            return NoContent();
        }


        [HttpPut("/api/Update/Lessons/{lessonId}")]
        public async Task<IActionResult> UpdateLessonDetails([FromRoute] int lessonId,[FromBody]UpdateLessonRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var lessonInDb = _lessonRepository.GetOne(expression:e=>e.Id == lessonId && e.ModuleId==request.ModuleId && e.Module.Course.ApplicationUserId==userId, includes:[
                e=>e.Module.Course
                ]);
            if (lessonInDb == null) return NotFound();

            if(!lessonInDb.Name.Equals(request.Name) && !string.IsNullOrEmpty(request.Name))
            {
                lessonInDb.Name = request.Name;
                await _lessonRepository.CommitAsync();
            }

            if (lessonInDb.ModuleId != request.ModuleId)
            {
                lessonInDb.ModuleId = request.ModuleId;
                await _lessonRepository.CommitAsync();
            }

            return NoContent();
        }
    }
}
