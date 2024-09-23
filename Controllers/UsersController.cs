
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DPowerAPI.Data;
using DPowerAPI.models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace DPowerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DPowerAPIContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public UsersController(DPowerAPIContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }
        

        // POST: api/Login
        [HttpPost]
        [Route("Login")]
        public async  Task<ActionResult<User>> Login(LoginModel login)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.userName == login.userName);
            if(user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user,user.password,login.password);
                if(result != null)
                {
                    var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", user.userName.ToString()),
                    new Claim("Password", user.password.ToString())
                };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: signIn
                        );
                    string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { Token = tokenValue, User = user });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                } 
            }
            
            return NoContent();
        }

        [Authorize]
        [HttpGet]
        // GET: api/Users
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {

            try
            {
                if (string.IsNullOrEmpty(user.password))
                {
                    return BadRequest("Password cannot be null or empty.");
                }
                // เข้ารหัสรหัสผ่าน
                user.password = _passwordHasher.HashPassword(user, user.password);

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.ID }, user);
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return (user);
            }
           
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
