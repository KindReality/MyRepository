using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // GET: Photos1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Photos.ToListAsync());
        }

        // GET: Photos1/Details/5
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

        // GET: Photos1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("PhotoID,Title,PhotoData,MimeType")] Photo photo, List<IFormFile> fileInputData)
        {
            if (ModelState.IsValid)
            {
                if (fileInputData.Count > 1)
                {
                    ModelState.AddModelError("PhotoData", "Please upload only one file.");
                }
                var formFile = fileInputData[0];
                var readStream = formFile.OpenReadStream();
                photo.PhotoData = new byte[formFile.Length];
                readStream.Read(photo.PhotoData, 0, (int)formFile.Length);
                photo.MimeType = formFile.ContentType;
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            return File(photo.PhotoData, photo.MimeType);
        }


        // GET: Photos1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photos1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoID,Title,PhotoData,MimeType")] Photo photo)
        {
            if (id != photo.PhotoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.PhotoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // GET: Photos1/Delete/5
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

        // POST: Photos1/Delete/5
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
