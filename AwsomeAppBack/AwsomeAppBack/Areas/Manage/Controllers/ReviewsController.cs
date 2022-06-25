using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AwsomeAppBack.DAL;
using AwsomeAppBack.Models;
using Microsoft.AspNetCore.Authorization;
using AwsomeAppBack.Utilities;
using AwsomeAppBack.Utilities.Extentions;

namespace AwsomeAppBack.Areas.Manage.Controllers
{
    [Area("Manage")][Authorize]
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Manage/Reviews
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reviews.ToListAsync());
        }


        // GET: Manage/Reviews/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                if (review.File == null)
                {
                    ModelState.AddModelError("File", "Must add file");
                    return View(review);
                }
                if (review.File.Length / 1024 > Consts.ReviewImgMaxSizeKb)
                {
                    ModelState.AddModelError("File", "Size cant be greater than:" + Consts.ReviewImgMaxSizeKb + "Kb");
                    return View(review);
                }
                if (!review.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("File", "Wrong Type");
                    return View(review);
                }
                review.Img = await review.File.SaveFile(Consts.ReviewImgPath, Consts.ReviewImgMaxLength);

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Manage/Reviews/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Review review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null)
                return NotFound();

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Review review)
        {
            if (!ModelState.IsValid) return View(review);
            Review dbreview = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == review.Id);
            if (dbreview == null) return NotFound();

            dbreview.Comment = review.Comment;
            dbreview.Name = review.Name;
            dbreview.Position = review.Position;
            if (review.File != null)
            {
                if (review.File.Length / 1024 < Consts.ReviewImgMaxSizeKb || !review.File.ContentType.Contains("image"))
                {
                    dbreview.Img.DeleteFile(Consts.ReviewImgPath);
                    dbreview.Img = await review.File.SaveFile(Consts.ReviewImgPath, Consts.ReviewImgMaxLength);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // POST: Manage/Reviews/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();
            review.Img.DeleteFile(Consts.ReviewImgPath);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
