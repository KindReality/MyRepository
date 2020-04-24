using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Photos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.PhotoID == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("PhotoID,Title")] Photo photo, List<IFormFile> fileInputData)
        {
            if (ModelState.IsValid)
            {
                if (fileInputData.Count > 1)
                {
                    ModelState.AddModelError("PhotoData", "Please upload only one file.");
                }
                var formFile = fileInputData[0];
                var readStream = formFile.OpenReadStream();
                var photoData = new byte[formFile.Length];
                readStream.Read(photoData, 0, photoData.Length);
                _context.Add(photo);
                await _context.SaveChangesAsync();
                StorageRepository.UploadBlob("photos", photo.PhotoID.ToString(), photoData);
                return RedirectToAction(nameof(Details), new { id = photo.PhotoID });
            }
            return View(photo);
        }

        public async Task<IActionResult> File(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.PhotoID == id);
            if (photo == null)
            {
                return NotFound();
            }
            var result = StorageRepository.DownloadBlob("photos", photo.PhotoID.ToString());
            return File(result.Blob, result.ContentType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.PhotoID == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.PhotoID == id);
        }
    }
}
