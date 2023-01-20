using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using NuGet.Versioning;
using testAPIs.Models;

namespace testAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _config;
        //public LoginController(IConfiguration config)
        //{
        //    _config = config;
        //}

        private readonly TestDbContext _context;

        public LoginController(TestDbContext context, IConfiguration config)
        {
            _context = context;

            _config = config;
        }

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUserTables()
        {
            return await _context.UserTables.ToListAsync();
        }

        // GET: api/Login/5
        [HttpGet("{id}")]
        [Authorize(Roles ="USER")]
        public async Task<ActionResult<UserTable>> GetUserTable(int id)
        {
            var userTable = await _context.UserTables.FindAsync(id);

            if (userTable == null)
            {
                return NotFound();
            }

            return userTable;
        }

        // GET: api/Login/
        //[HttpGet("{email}/{password}")]
        //public async Task<ActionResult<UserTable>> GetUserTable(string email,string password)
        //{
        //    var userDetails=new UserTable();
        //    //var userDetailsList=new List<UserTable>();
        //    //userDetailsList= await _context.UserTables.ToListAsync();

        //    //Console.WriteLine(userDetails+"from 38");
        //    var userTable = await _context.UserTables.Where(x =>  x.Email == email && x.Password == password).FirstOrDefaultAsync();
        //    /*var userTable1 = await _context.UserTables.FindBestMatch(id);*/

        //    if (userTable != null && userTable.Password==password)
        //    {
        //        //userDetails.Id = id;
        //        userDetails.FirstName= userTable.FirstName;
        //        userDetails.LastName = userTable.LastName;
        //        userDetails.PhoneNumber = userTable.PhoneNumber;
        //        userDetails.Email = userTable.Email;
        //        userDetails.AadharNumber= userTable.AadharNumber;
        //        userDetails.Passport= userTable.Passport;

        //        return userDetails;   
        //    }

        //    return NotFound();
        //}

        // PUT: api/Login/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, UserTable userTable)
        {

            if (id != userTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
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

        

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UserLogin(LogInReqBody logInReqBody)
        {
            //var userDetails = new UserTable();

            //var userTable = await _context.UserTables.Where(x => x.Email == logInReqBody.email && x.Password == logInReqBody.password).FirstOrDefaultAsync();

            //var user = Authenticate(logInReqBody);
            var user = _context.UserTables.SingleOrDefault(x => x.Email == logInReqBody.email && x.Password == logInReqBody.password);
            //Console.WriteLine(user);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);

                //userDetails.Id = id;
                //userDetails.FirstName = userTable.FirstName;
                //userDetails.LastName = userTable.LastName;
                //userDetails.PhoneNumber = userTable.PhoneNumber;
                //userDetails.Email = userTable.Email;
                //userDetails.AadharNumber = userTable.AadharNumber;
                //userDetails.Passport = userTable.Passport;

                //return userDetails;
            }

            return NotFound("Invalid User Name or Password");
        }

        // To generate token
        private string GenerateToken(UserTable user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        //private UserTable Authenticate(LogInReqBody userLogin)
        //{
        //    var currentUser = _context.UserTables.FirstOrDefault(x => x.Email.ToLower() ==
        //        userLogin.email.ToLower() && x.Password == userLogin.password);
        //    if (currentUser != null)
        //    {
        //        return currentUser;
        //    }
        //    return null;
        //}

        // POST: signUp
        [HttpPost("/signUp")]
        public async Task<ActionResult<UserTable>> UserSignUp(UserTable userTable)
        {
            //try
            //{
                _context.UserTables.Add(userTable);
                await _context.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    var sqlException = ex.InnerException;

            //    //return BadRequest(sqlException + " \n get excep "+ex.GetBaseException+" \n type"+ex.GetType+" \n "+ ex.InnerException.Message);

            //    if (sqlException.Message.Contains("Violation of UNIQUE KEY constraint") && sqlException.Message.Contains("EmailColumn"))
            //    {
            //        return BadRequest("Cannot insert duplicate values.");
            //    }else 
            //    //if(sqlException.Message.Contains("Violation of UNIQUE KEY constraint") && sqlException.Message.Contains("PhoneNumber"))
                
            //    {
            //        //return BadRequest("Error while saving data.");
            //        return BadRequest(ex);
            //    }
            //}

            return CreatedAtAction("GetUserTable", new { id = userTable.Id }, userTable);
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTable(int id)
        {
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(int id)
        {
            return _context.UserTables.Any(e => e.Id == id);
        }
    }

    

}
