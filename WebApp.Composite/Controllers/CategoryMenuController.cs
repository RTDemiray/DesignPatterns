using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseProject.Composite;
using BaseProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Composite.Controllers
{
    [Authorize]
    public class CategoryMenuController : Controller
    {
        private readonly DataContext _context;

        public CategoryMenuController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // category => bookcomposite
            // book => bookcomponent
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var categories = await _context.Categories.Include(x => x.Books).Where(x => x.UserId == userId)
                .OrderBy(x => x.Id).ToListAsync();

            var menu = GetMenus(categories, new Category {Name = "TopCategory", Id = 0},
                new BookComposite(0, "TopMenu")); //ReferenceId 0 olduğu için Id 0 değerinde bir kategori oluşturuyoruz.

            ViewBag.menu = menu;
            ViewBag.selectList = menu.Components.SelectMany(x => ((BookComposite) x).GetSelectListItem(""));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int categoryId, string bookName)
        {
            await _context.Books.AddAsync(new Book {CategoryId = categoryId, Name = bookName});
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public BookComposite GetMenus(List<Category> categories, Category topCategory, BookComposite topBookComposite, BookComposite last = null)
        {
            categories.Where(x => x.ReferenceId == topCategory.Id).ToList().ForEach(categoryItem =>
            {
                var bookComposite = new BookComposite(categoryItem.Id, categoryItem.Name);
                
                categoryItem.Books.ToList().ForEach(bookItem =>
                {
                    bookComposite.Add(new BookComponent(bookItem.Id, bookItem.Name));
                });

                if (last != null)
                {
                    last.Add(bookComposite);
                }
                else
                {
                    topBookComposite.Add(bookComposite);
                }

                GetMenus(categories, categoryItem, topBookComposite, bookComposite);
            });
            return topBookComposite;
        }
    }
}