using core2Bowling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Controllers
{

    [Authorize(Policy = "AdminGroup")]
    //[Authorize(Roles = "Manager")]
    public class UserIdentitiesController : Controller
    {
        private readonly AccountContext _context;

        public UserIdentitiesController(AccountContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: UserIdentities
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserIdentities.ToListAsync());
        }

        // GET: UserIdentities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdentity = await _context.UserIdentities
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userIdentity == null)
            {
                return NotFound();
            }

            return View(userIdentity);
        }

        [Authorize(Roles = "Admin")]
        // GET: UserIdentities/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: UserIdentities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Password,NicName,UserGroup,InActivity")] UserIdentity userIdentity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userIdentity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userIdentity);
        }

        // GET: UserIdentities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdentity = await _context.UserIdentities.SingleOrDefaultAsync(m => m.UserId == id);
            if (userIdentity == null)
            {
                return NotFound();
            }
            return View(userIdentity);
        }

        // POST: UserIdentities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdentity = await _context.UserIdentities.SingleOrDefaultAsync(m => m.UserId == id);
            if (userIdentity == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<UserIdentity>(userIdentity, "",
                u => u.NicName, u => u.Password, u => u.Role, u => u.UserGroup, u=>u.InActivity))
            {


                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = id });
                }

                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "UserIdentity_Edit_Post_Error");

                    throw;

                }
            }
            return View(userIdentity);
        }

        [Authorize(Roles = "Admin")]
        // GET: UserIdentities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdentity = await _context.UserIdentities
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userIdentity == null)
            {
                return NotFound();
            }

            return View(userIdentity);
        }

        [Authorize(Roles = "Admin")]
        // POST: UserIdentities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userIdentity = await _context.UserIdentities.SingleOrDefaultAsync(m => m.UserId == id);
            _context.UserIdentities.Remove(userIdentity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserIdentityExists(string id)
        {
            return _context.UserIdentities.Any(e => e.UserId == id);
        }
    }
}
