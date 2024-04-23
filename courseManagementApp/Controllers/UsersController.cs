using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using courseManagementApi.DBContexts;
using courseManagementApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace courseManagementApi.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CourseContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(CourseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
        }


        /*
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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
       

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }
         */
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users user)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception($"One or more validation failed, Kindly check the data provided");
            }

            if(_context.Users.Any(u => u.UserEmail == user.UserEmail)) {
                return BadRequest("User already exist, kindly login.");
            }

            var newUser = new Users { Username = user.Username, UserEmail = user.UserEmail, Password = BCrypt.Net.BCrypt.HashPassword(user.Password) };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(Users userCredential)
        {
            var user = await _context.Users.SingleOrDefaultAsync(a => a.UserEmail == userCredential.UserEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userCredential.Password, user.Password))
            {
                return Unauthorized();
            }

            // generating session token for user
            var securityKey = new SymmetricSecurityKey(
               Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString()));

            //check for null values
            if (!string.IsNullOrEmpty(user.Username))
            {
                claimsForToken.Add(new Claim("given_name", user.Username));
            }
            else
            {
                claimsForToken.Add(new Claim("given_name", "DefaultUsername"));
            }

            if(!string.IsNullOrEmpty(user.Role))
            {
                claimsForToken.Add(new Claim("role", user.Role));
            }
            else
            {
                claimsForToken.Add(new Claim("role", "USER"));
            }

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(5),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

         
        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
