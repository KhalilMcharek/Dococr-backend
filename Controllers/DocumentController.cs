using Documents_OCR_back.Models.DTOs;
using Documents_OCR_back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Documents_OCR_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;

        public DocumentController(DocumentService documentService)
        {
            _documentService = documentService;
        }
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument([FromForm] IFormFile file, [FromForm] string? documentName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier envoyé.");

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var fileName = !string.IsNullOrEmpty(documentName) ? documentName : Path.GetFileName(file.FileName);

            var textExtracted = "Texte extrait";

            var doc = await _documentService.UploadDocumentAsync(fileName, textExtracted, userId);

            return Ok(new { message = "Document uploadé", document = doc });
        }



        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var docs = await _documentService.ListeDocumentAsync(userId);
            return Ok(docs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var doc = await _documentService.DocumentById(id);
            if (doc == null)
                return NotFound(new { message = "Document introuvable" });

            return Ok(doc);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return Ok(new { message = "Document supprimé avec succès" });
        }
    }
}
