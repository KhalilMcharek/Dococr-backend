using Documents_OCR_back.Data;
using Documents_OCR_back.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace Documents_OCR_back.Services

{
    public class DocumentService
    {
        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }      
         //upload 
        public async Task<Document> UploadDocumentAsync(string filename, string textExtracted, int userId)
        {
            var Doc = new Document
            { FileName = filename, TextExtracted = textExtracted, CorrectedText = "", UserId = userId, UploadedAt = DateTime.Now };
            _context.Documents.Add(Doc);
            await _context.SaveChangesAsync();
            return Doc;
        }
        // historique ( liste)
        public async Task<List<Document>> ListeDocumentAsync( int userId)
        {
            return await _context.Documents.Where(d => d.UserId == userId).ToListAsync();
        }
         //document par id 
        public async Task <Document> DocumentById ( int id)
        {
            return await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
        }
        
        //supprimer 
        public async Task DeleteDocumentAsync(int id)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null)
                throw new Exception("Document not found");

            _context.Documents.Remove(doc);
            await _context.SaveChangesAsync();
        }
    }
}
