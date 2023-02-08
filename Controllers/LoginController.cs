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
using testAPIs.DTO;

namespace testAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _config;
        //private readonly IHttpContextAccessor _accessor;

        //public LoginController(IConfiguration config)
        //{
        //    _config = config;
        //}

        private readonly TestDbContext _context;

        public LoginController(TestDbContext context, IConfiguration config)
        {
            _context = context;

            _config = config;

            //_accessor = accessor;
        }

        // GET: api/Login
        [HttpGet]
        public async Task<List<GetUserTableDTO>> GetUserTables()
        {
            List< GetUserTableDTO> getUserTableDTO = new();
            //return await _context.UserTables.ToListAsync();
            foreach(UserTable userTable in await _context.UserTables.ToListAsync())
            {
                GetUserTableDTO getUserTableDTO1 = new()
                {
                    Id = userTable.Id,
                    FirstName = userTable.FirstName,
                    LastName = userTable.LastName,
                    Email = userTable.Email,
                    PhoneNumber = userTable.PhoneNumber,
                    AadharNumber = DecodeFrom64(userTable.AadharNumber),
                    //getUserTableDTO1.Passport = userTable.Passport;
                    Passport = DecodeFrom64(userTable.Passport),
                    Role = userTable.Role,
                    CreatedDate = userTable.CreatedDate
                };
                getUserTableDTO.Add(getUserTableDTO1);
            }

            return getUserTableDTO;

        }

        //this function Convert to Encord Passport
        private static string EncodeToBase64(string passportNumber)
        {
            //try
            //{
            if (passportNumber != null)
            {
                byte[] encData_byte = new byte[passportNumber.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(passportNumber);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            return passportNumber;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error in base64Encode" + ex.Message);
            //}
        }

        //this function Convert to Decord passportNumber
        private static string DecodeFrom64(string encodedData)
        {
            if (encodedData != null)
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encodedData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            return encodedData;
        }

        // GET: api/Login/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetUserTableDTO>> GetUserTable(int id)
        {
            var userTable = await _context.UserTables.FindAsync(id);

            var userDetails = new GetUserTableDTO();

            if (userTable == null)
            {
                return NotFound();
            }

            //return userTable;
            userDetails.Id = id;
            userDetails.FirstName = userTable.FirstName;
            userDetails.LastName = userTable.LastName;
            userDetails.PhoneNumber = userTable.PhoneNumber;
            userDetails.Email = userTable.Email;
            userDetails.AadharNumber = DecodeFrom64(userTable.AadharNumber);  
            //userDetails.Passport = userTable.Passport;
            userDetails.Passport = DecodeFrom64(userTable.Passport);
            userDetails.Role = userTable.Role;

            return userDetails;
        }

        // PUT: api/Login/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, UpdateUserTableDTO userTableDTO)
        {
            //var roles = new AuthorizeAttribute();

            if (id != userTableDTO.Id)
            {
                return BadRequest("Id not matching");
            }

            var userDetails = await _context.UserTables.FindAsync(id);
            if (userDetails is not null)
            {
                //userDetails.Id = id;
                userDetails.FirstName = userTableDTO.FirstName;
                userDetails.LastName = userTableDTO.LastName;
                userDetails.PhoneNumber = userTableDTO.PhoneNumber;
                //userDetails.Email = userTableDTO.Email.ToLower();
                userDetails.AadharNumber = EncodeToBase64(userTableDTO.AadharNumber);
                //userDetails.Password = userTableDTO.Password;
                userDetails.Passport = EncodeToBase64(userTableDTO.Passport);
                //userDetails.Role = userTableDTO.Role;
                //Console.WriteLine(roles.Roles == null);
                //Console.ReadLine();
                //if (roles.Roles == "ADMIN")
                //{
                //    userDetails.Email = userTableDTO.Email;
                //}
            }

            _context.Entry(userDetails).State = EntityState.Modified;

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
        public ActionResult UserLogin(LogInReqBody logInReqBody)
        {
            var userActivityTable = new UserActivityTable();
            //var roles = new AuthorizeAttribute();
            //Console.WriteLine(roles);
            //Console.ReadLine();
            //Console.WriteLine(roles.Roles+" Roles");
            //Console.ReadLine();
            //var userTable = await _context.UserTables.Where(x => x.Email == logInReqBody.email && x.Password == logInReqBody.password).FirstOrDefaultAsync();

            var user = _context.UserTables.SingleOrDefault(x => x.Email == logInReqBody.Email.ToLower() && x.Password == logInReqBody.Password);

            if (user != null)
            {
                var token = GenerateToken(user);
                userActivityTable.UserId = user.Id;
                userActivityTable.UserName = user.Email;
                userActivityTable.GeneratedToken = token;
                userActivityTable.Ipaddress = HttpContext.Connection.RemoteIpAddress.ToString(); 

                _context.UserActivityTables.Add(userActivityTable);
                _context.SaveChanges();

                return Ok(token);

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

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384Signature);
            
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        // POST: signUp
        [Route("/signUp")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<GetUserTableDTO>> UserSignUp(CreateUserTableDTO[] userTableDto)
        {
            //var address = new UserAddressTable();
            //for(int i=0; i< userTableDto.Length; i++)
            //{
            //    var address = new UserAddressTable();
            //    var userDetails = new UserTable();
            //    //userDetails.Id = id;
            //    userDetails.FirstName = userTableDto[i].FirstName;
            //    userDetails.LastName = userTableDto[i].LastName;
            //    userDetails.PhoneNumber = userTableDto[i].PhoneNumber;
            //    userDetails.Email = userTableDto[i].Email.ToLower();
            //    userDetails.AadharNumber = EncodeToBase64(userTableDto[i].AadharNumber);
            //    userDetails.Password = userTableDto[i].Password;
            //    userDetails.Passport = EncodeToBase64(userTableDto[i].Passport);
            //    _context.UserTables.Add(userDetails);
            //    await _context.SaveChangesAsync();

            //    //Adding address
            //    address.Id = userDetails.Id;
            //    address.AddressLine1 = userTableDto[i].Address.AddressLine1;
            //    address.AddressLine2 = userTableDto[i].Address.AddressLine2;
            //    address.AddressLine3 = userTableDto[i].Address.AddressLine3;
            //    address.CountryName = userTableDto[i].Address.CountryName;
            //    address.UserName = userDetails.Email;
            //    address.StateName = userTableDto[i].Address.StateName;
            //    address.Zipcode = userTableDto[i].Address.Zipcode;
            //    _context.UserAddressTables.Add(address);

            //    await _context.SaveChangesAsync();

            //}

            //Simplified using foreach
            foreach(CreateUserTableDTO userDetailsFromBody in userTableDto)
            {
                
                //Adding User details
                var userDetails = new UserTable
                {
                    FirstName = userDetailsFromBody.FirstName,
                    LastName = userDetailsFromBody.LastName,
                    PhoneNumber = userDetailsFromBody.PhoneNumber,
                    Email = userDetailsFromBody.Email,
                    Password = userDetailsFromBody.Password,
                    Passport = EncodeToBase64(userDetailsFromBody.Passport),
                    AadharNumber = EncodeToBase64(userDetailsFromBody.AadharNumber)
                };
                _context.UserTables.Add(userDetails);
                await _context.SaveChangesAsync();

                //Adding address
                var address = new UserAddressTable
                {
                    Id = userDetails.Id,
                    UserName = userDetails.Email,
                    AddressLine1 = userDetailsFromBody.Address.AddressLine1,
                    AddressLine2 = userDetailsFromBody.Address.AddressLine2,
                    AddressLine3 = userDetailsFromBody.Address.AddressLine3,
                    CountryName = userDetailsFromBody.Address.CountryName,
                    StateName = userDetailsFromBody.Address.StateName,
                    Zipcode = userDetailsFromBody.Address.Zipcode
                };
                _context.UserAddressTables.Add(address);
                await _context.SaveChangesAsync();
            }
            //userDetails.Role = userTable.Role;
            //try
            //{
            //userTable.Passport = EncodeToBase64(userTableDto.Passport);
            //_context.UserTables.Add(userDetails);
            //await _context.SaveChangesAsync();
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

            //return CreatedAtAction("GetUserTable", new { id = userDetails.Id }, userDetails);
            //return Ok(GetUserTable(userDetails.Id).Result.Value);
            return Ok("User deatils added ");
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUserTable(int id)
        {
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return Ok("User deleted Successfully");
        }

        private bool UserTableExists(int id)
        {
            return _context.UserTables.Any(e => e.Id == id);
        }
    }

    

}
