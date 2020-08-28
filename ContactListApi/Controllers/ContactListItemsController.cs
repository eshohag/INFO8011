using ContactListApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactListApi.Controllers
{
    [Route("api/contactListItems")]
    [ApiController]
    public class ContactListItemsController : ControllerBase
    {
        private readonly ContactListContext _context;

        public ContactListItemsController(ContactListContext context)
        {
            _context = context;
        }       
        // GET: api/ContactListItems
        [HttpGet]
        public IEnumerable<ContactListItem> GetContactListItems()
        {
            return _context.ContactListItems;
        }

        // GET: /api/ContactListItems/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactListItem(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactListItem = await _context.ContactListItems.FindAsync(id);

            if (contactListItem == null)
            {
                return NotFound();
            }

            return Ok(contactListItem);
        }

        // PUT: api/ContactListItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactListItem([FromRoute] long id, [FromBody] ContactListItem contactListItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(contactListItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(contactListItem);
        }

        // POST: api/ContactListItems
        [HttpPost]
        public async Task<IActionResult> PostContactListItem(ContactListItem contactListItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ContactListItems.Add(contactListItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactListItem", new { id = contactListItem.Id }, contactListItem);
        }

        // DELETE: api/ContactListItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactListItem([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactListItem = await _context.ContactListItems.FindAsync(id);
            if (contactListItem == null)
            {
                return NotFound();
            }

            _context.ContactListItems.Remove(contactListItem);
            await _context.SaveChangesAsync();

            return Ok(contactListItem);
        }
    }
}