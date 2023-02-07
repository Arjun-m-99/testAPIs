using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testAPIs.Models;

namespace testAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressTablesController : ControllerBase
    {
        private readonly TestDbContext _context;

        public UserAddressTablesController(TestDbContext context)
        {
            _context = context;
        }

        // GET: api/UserAddressTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAddressTable>>> GetUserAddressTables()
        {
            return await _context.UserAddressTables.ToListAsync();
        }

        // GET: api/UserAddressTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAddressTable>> GetUserAddressTable(int id)
        {
            var userAddressTable = await _context.UserAddressTables.FindAsync(id);

            if (userAddressTable == null)
            {
                return NotFound();
            }

            return userAddressTable;
        }

        // PUT: api/UserAddressTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAddressTable(int id, UserAddressTable userAddressTable)
        {
            if (id != userAddressTable.RowNumber)
            {
                return BadRequest();
            }

            _context.Entry(userAddressTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAddressTableExists(id))
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

        // POST: api/UserAddressTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAddressTable>> PostUserAddressTable(UserAddressTable userAddressTable)
        {
            _context.UserAddressTables.Add(userAddressTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAddressTable", new { id = userAddressTable.RowNumber }, userAddressTable);
        }

        // DELETE: api/UserAddressTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAddressTable(int id)
        {
            var userAddressTable = await _context.UserAddressTables.FindAsync(id);
            if (userAddressTable == null)
            {
                return NotFound();
            }

            _context.UserAddressTables.Remove(userAddressTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserAddressTableExists(int id)
        {
            return _context.UserAddressTables.Any(e => e.RowNumber == id);
        }
    }
}
