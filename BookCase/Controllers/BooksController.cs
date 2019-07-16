using BookCase.Data;
using BookCase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookCase.Controllers
{
    // Makes certain that the user is logged in before they can see anything else. Redirects the user to the login page.
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var applicationDbContext = _context.Book
                                                .Include(b => b.Author)
                                                .Include(b => b.Owner)
                                                .Where(b => b.OwnerId == currentUser.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName");
            //ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Isbn,Genre,Title,PublishDate,AuthorId")] Book book)
        {
            // Removes required properties that we aren't setting in this post so the post is successful / model state is valid now. 
            ModelState.Remove("Author");
            ModelState.Remove("Owner");
            ModelState.Remove("OwnerId");

            // Removed the ability to set the ownerId with the form. Instead, the ownerId is set by finding the currently logged in user (whoever creates the book).

            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                book.OwnerId = currentUser.Id;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            /*ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Id", book.OwnerId);*/
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            //ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Id", book.OwnerId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Isbn,Genre,Title,PublishDate,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            ModelState.Remove("OwnerId");
            ModelState.Remove("Owner");
            ModelState.Remove("Author");
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                    book.OwnerId = currentUser.Id;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            //ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Id", book.OwnerId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
