using DemoSvelte.Models;
using DemoSvelte.Models.ViewModels;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        //private readonly IAppUserService appUserService;
        //private readonly IMongoCollection<AppUser> _appusers;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthentificationController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,SignInManager<AppUser> signInManager)
        {
            //var database = mongoClient.GetDatabase(settings.DatabaseName);
            //_appusers = database.GetCollection<AppUser>(settings.AppUserCollectionName);
            // this.appUserService = appUserService;
            _userManager= userManager;
            _roleManager= roleManager;
            _signInManager = signInManager;
             
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginAppUserVM appUser)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(appUser.Email);

                if (user == null)
                {
                    return BadRequest("User does not exist");
                }


                var result = await _signInManager.CheckPasswordSignInAsync(user, appUser.Password, false);

                if (!result.Succeeded)
                {
                    return BadRequest("Invalid username Or email");
                }

                var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Sub , user.Id.ToString() ),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1swek3u4uo2u4a6e"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(10);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds);
                return Ok(
                    new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterAppUserVM registerAppUserVM)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(registerAppUserVM.Email);
                if (userExists != null) {
                    return BadRequest("User already exists.");
                }

                userExists = new AppUser
                {
                    Name = registerAppUserVM.Name,
                    Email = registerAppUserVM.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = registerAppUserVM.Email,
                    
                };

                var createUserResult = await _userManager.CreateAsync(userExists, registerAppUserVM.Password);
                if (!createUserResult.Succeeded)
                {
                    return BadRequest("Registration Failed");
                }

                var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, registerAppUserVM.UserRole);
                if(!addUserToRoleResult.Succeeded) {
                    return BadRequest("User Created but could not add user role");
                
                }
                
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }

        [HttpPost]
        [Route("CreateUserRole")]

        public  async Task <IActionResult> CreateRole([FromBody] AppRoleVm model)
        {
            var appRole = new AppRole { Name = model.Role};
            var createRole = await _roleManager.CreateAsync(appRole);

            return Ok("New role Created succesfully");
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(AppUserVM appUser)
        //{
        //    if (_appusers.Find(e => e.Email == appUser.Email).FirstOrDefault();)
        //    {
        //        return BadRequest("User already exists.");
        //    }

        //    CreatePasswordHash(request.Password,
        //         out byte[] passwordHash,
        //         out byte[] passwordSalt);

        //    var user = new User
        //    {
        //        Email = request.Email,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        VerificationToken = CreateRandomToken()
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return Ok("User successfully created!");
        //}
        //[HttpGet("GetAppUsers")]
        //public ActionResult<List<AppUser>> GetAppUsers()
        //{
        //    return appUserService.Get();
        //}

        //[HttpGet("GetUserAppUserbyId/{id}")]
        //public ActionResult<AppUser> GetAppUserbyId(string id)
        //{
        //    var appUser = appUserService.Get(id);

        //    if (appUser == null)
        //    {
        //        return NotFound($"AppUser with Id = {id} not found");
        //    }

        //    return appUser;
        //}


        //[HttpPost("RegisterAppUser")]
        //public ActionResult<AppUser> Post([FromBody] AppUser appUser)
        //{
        //    appUserService.Create(appUser);

        //    return CreatedAtAction(nameof(GetAppUsers), new { id = appUser.Id }, appUser);
        //}


        //[HttpPut("UpdateAppUser/{id}")]
        //public ActionResult Put(string id, [FromBody] AppUser appUser)
        //{
        //    var existingProperty = appUserService.Get(id);

        //    if (existingProperty == null)
        //    {
        //        return NotFound($"AppUser with Id = {id} not found");
        //    }

        //    appUserService.Update(id, appUser);

        //    return NoContent();
        //}


        //[HttpDelete("Delete AppUser/{id}")]
        //public ActionResult DeleteAppUser(string id)
        //{
        //    var appUser = appUserService.Get(id);

        //    if (appUser == null)
        //    {
        //        return NotFound($"AppUser with Id = {id} not found");
        //    }

        //    appUserService.Delete(appUser.Id);

        //    return Ok($"AppUser with Id = {id} deleted");
        //}
    }
}
