using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos.ResumeDtos;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ResumesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetResumes()
        {
            var resumes = _context.Resumes.ToList();
            if (resumes == null || !resumes.Any())
            {
                return NotFound("No resumes found.");
            }

            ResultResumeDto resultResumeDto = new ResultResumeDto
            {
                ResumeId = resumes.FirstOrDefault()?.ResumeId ?? 0,
                PdfFile = resumes.FirstOrDefault()?.PdfFile
            };
            return Ok(resultResumeDto);
        }

        [HttpGet("get-resume-file")]
        public IActionResult GetResumeFile()
        {
            var pdfEntity = _context.Resumes.FirstOrDefault();
            if (pdfEntity == null || pdfEntity.PdfFile == null)
                return NotFound();

            Response.Headers.Add("Content-Disposition", "inline; filename=resume.pdf");
            return File(Convert.FromBase64String(pdfEntity.PdfFile), "application/pdf");
        }

        [HttpPost("create-resume")]
        public IActionResult CreateResume(CreateResumeDto createResumeDto)
        {
            if (createResumeDto == null || createResumeDto.PdfFile == null)
            {
                return BadRequest("Resume cannot be null.");
            }

            // Convert the PDF file to a byte array
            byte[] pdfBytes;
            using (var memoryStream = new MemoryStream())
            {
                createResumeDto.PdfFile.CopyTo(memoryStream);
                pdfBytes = memoryStream.ToArray();
            }

            // Check if a resume already exists
            var existingResume = _context.Resumes.FirstOrDefault();
            if (existingResume != null)
            {
                existingResume.PdfFile = Convert.ToBase64String(pdfBytes); // Store as Base64 string if needed
                _context.Resumes.Update(existingResume);
            }
            else
            {
                var newResume = new Entities.Resume { PdfFile = Convert.ToBase64String(pdfBytes) };
                _context.Resumes.Add(newResume);
            }

            _context.SaveChanges();

            return Ok("Resume created successfully.");
        }

        [HttpPut("update-resume")]
        public IActionResult UpdateResume([FromForm] UpdateResumeDto updateResumeDto)
        {
            if (updateResumeDto == null || updateResumeDto.PdfFile == null)
            {
                return BadRequest("Resume cannot be null.");
            }

            byte[] pdfBytes;

            // Pdf dosyasını MemoryStream'e kopyala
            using (var inputStream = updateResumeDto.PdfFile.OpenReadStream())
            using (var inputMemoryStream = new MemoryStream())
            {
                inputStream.CopyTo(inputMemoryStream);
                inputMemoryStream.Position = 0; // Baştan okumak için pozisyonu sıfırla

                // PdfSharp ile PDF'i aç ve başlığı güncelle
                var document = PdfReader.Open(inputMemoryStream, PdfDocumentOpenMode.Modify);
                document.Info.Title = "Ozgecmis";

                using (var outputMemoryStream = new MemoryStream())
                {
                    document.Save(outputMemoryStream);
                    pdfBytes = outputMemoryStream.ToArray();
                }
            }

            // Veritabanındaki kaydı bul
            var existingResume = _context.Resumes.FirstOrDefault();
            if (existingResume == null)
            {
                return NotFound("No resume found to update.");
            }

            existingResume.PdfFile = Convert.ToBase64String(pdfBytes); // Veritabanında string olarak saklanıyorsa base64 string yap
            _context.Resumes.Update(existingResume);
            _context.SaveChanges();

            return Ok("Resume updated successfully.");
        }

    }
}
