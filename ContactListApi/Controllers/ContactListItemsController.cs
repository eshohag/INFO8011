using ContactListApi.Models;
using ContactListApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

            if (id != contactListItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactListItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactListItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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

        private bool ContactListItemExists(long id)
        {
            return _context.ContactListItems.Any(e => e.Id == id);
        }
    }
}