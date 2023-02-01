using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using NuGet.Versioning;
using testAPIs.DTO;
using testAPIs.Models;

namespace testAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivityTablesController : ControllerBase
    {
        private readonly TestDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public UserActivityTablesController(TestDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;

            _accessor = accessor;
        }

        // GET: api/UserActivityTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserActivityTable>>> GetUserActivityTables()
        {
            return await _context.UserActivityTables.ToListAsync();
        }

        // GET: api/UserActivityTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserActivityTable>> GetUserActivityTable(int id)
        {
            var userActivityTable = await _context.UserActivityTables.FindAsync(id);

            if (userActivityTable == null)
            {
                return NotFound();
            }

            return userActivityTable;
        }
        [HttpGet("getDataByUserId/{id}")]
        public ActionResult<GetUserActivityTableDTO> GetUserActivityTableByID(int id)
        {

            var userActivityTable =_context.UserActivityTables.Where(x => x.UserId == id);

            if (userActivityTable == null || userActivityTable.IsNullOrEmpty())
            {
                return NotFound("No data found with ID-"+id);
            }
            
            return Ok(userActivityTable);
        }

        // PUT: api/UserActivityTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserActivityTable(int id, UserActivityTable userActivityTable)
        //{
        //    if (id != userActivityTable.RowNumber)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userActivityTable).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserActivityTableExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/UserActivityTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserActivityTable>> PostUserActivityTable(UserActivityTable userActivityTable)
        //{
        //    _context.UserActivityTables.Add(userActivityTable);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserActivityTable", new { id = userActivityTable.RowNumber }, userActivityTable);
        //}

        // DELETE: api/UserActivityTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserActivityTable(int id)
        {
            var userActivityTable = await _context.UserActivityTables.FindAsync(id);
            if (userActivityTable == null)
            {
                return NotFound();
            }

            _context.UserActivityTables.Remove(userActivityTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserActivityTableExists(int id)
        {
            return _context.UserActivityTables.Any(e => e.RowNumber == id);
        }
    }
}
