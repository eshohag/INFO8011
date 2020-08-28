using ContactListApi.Models;
using ContactListApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListApi.Controllers
{
    public class ContactListController : Controller
    {
        private readonly ContactListContext _context;
        private readonly IContactListItemRepository _contactListItemRepository;

        public ContactListController(ContactListContext context, IContactListItemRepository contactListItemRepository)
        {
            _context = context;
            _contactListItemRepository = contactListItemRepository;
        }

        public IActionResult Index()
        {
            var list = _contactListItemRepository.All().ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new ContactListItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactListItem model)
        {
            var isExistPhone = _contactListItemRepository.IsExist(a => a.Phone.Trim() == model.Phone.Trim());
            var isExistEmail = _contactListItemRepository.IsExist(a => a.Email.Trim() == model.Email.Trim());
            if (isExistPhone)
            {
                ModelState.AddModelError("Phone", "Already Phone no exist!");
            }
            if (isExistEmail)
            {
                ModelState.AddModelError("Email", "Already Email no exist!");
            }
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ContactList/Edit/1
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactListItem = await _context.ContactListItems.FindAsync(id);
            if (contactListItem == null)
            {
                return NotFound();
            }
            var model = new List<ContactListItem>();
            model.Add(contactListItem);
            return View(model);
        }

        // POST: ContactList/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ContactListItem contactListItem)
        {
            if (id != contactListItem.Id)
            {
                return NotFound();
            }
            var isExistPhone = _contactListItemRepository.IsExist(a => a.Phone.Trim() == contactListItem.Phone.Trim() && a.Id != id);
            var isExistEmail = _contactListItemRepository.IsExist(a => a.Email.Trim() == contactListItem.Email.Trim() && a.Id != id);
            if (isExistPhone)
            {
                ModelState.AddModelError("Phone", "Already Phone no exist!");
            }
            if (isExistEmail)
            {
                ModelState.AddModelError("Email", "Already Email no exist!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactListItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactListItemExists(contactListItem.Id))
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
            return View(contactListItem);
        }

        // GET: ContactList/Delete/1
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactListItem = await _context.ContactListItems.FirstOrDefaultAsync(m => m.Id == id);
            if (contactListItem == null)
            {
                return NotFound();
            }
            return View(contactListItem);
        }

        // POST: ContactList/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var contactListItem = await _context.ContactListItems.FindAsync(id);
            _context.ContactListItems.Remove(contactListItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactListItemExists(long id)
        {
            return _context.ContactListItems.Any(e => e.Id == id);
        }
    }
}
