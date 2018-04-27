using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MomensoBackend.Data;
using MomensoBackend.Models;
using MomensoBackend.Models.AccountModels;
using RowcallBackend.Models;
using RowcallBackend.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MomensoBackend.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("SiteCorsPolicy")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [Authorize(Roles = "Student")]
        public IActionResult Test()
        {
            return Json("Hello");
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var role = roles.First();

                        return Json(new JsonResponse(true, await GenerateJwtToken(model.Email, user, role)));
                    }
                }
                return Json(new JsonResponse(false, "Invalid login attempt."));
            }
            var modelError = ModelState.Values.SelectMany(x => x.Errors).First().ErrorMessage;

            return Json(new JsonResponse(false, modelError));
        }

        [HttpPost]
        public async Task<object> StudentLogin([FromBody] StudentLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.Include(x => x.UserClass).SingleOrDefault(x => x.Email == model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        
                        var token = _dbContext.Token.Include(x => x.ClassRoom).SingleOrDefault(x => x.TokenValue == model.TokenValue);

                        foreach (var classRoom in user.UserClass)
                        {
                            if(classRoom.ClassRoomId == token.ClassId)
                            {
                                DateTime timeNow = DateTime.Now;
                                DateTime durationTime = token.CreatedDateTime.AddMinutes(30);

                                if (timeNow <= durationTime)
                                {
                                    UserToken userToken = new UserToken();
                                    userToken.ApplicationUserId = user.Id;
                                    userToken.TokenId = token.Id;

                                    return Ok(userToken);
                                }
                                else
                                {
                                    return Json(new JsonResponse(false, "Duration of the token has expired."));
                                }
                            }
                            else
                            {
                                return Json(new JsonResponse(false, "Student does not belong to this class."));
                            }
                        } 
                    }
                }
                return Json(new JsonResponse(false, "Invalid login attempt."));
            }
            var modelError = ModelState.Values.SelectMany(x => x.Errors).First().ErrorMessage;

            return Json(new JsonResponse(false, modelError));
        }

        private bool IsTokenValidForStudent()
        {
            bool result = false;

            return result;
        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Lets add user to a role: 
                    var role = ""; 
                    if (model.Teacher)
                    {
                        await _userManager.AddToRoleAsync(user, "Teacher");
                        role = "Teacher";
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Student");
                        role = "Student"; 
                    }
                     

                    await _signInManager.SignInAsync(user, false);
                    return Json(new JsonResponse(true, await GenerateJwtToken(model.Email, user, role)));
                }
                var errorMsg = result.Errors.First().Description;
                return Json(new JsonResponse(false, errorMsg));
            }
            var modelError = ModelState.Values.SelectMany(x => x.Errors).First().ErrorMessage;
            return Json(new JsonResponse(false, modelError));
        }

        // Generates a Json Web Token and returns it to the user. The user should store and use this in every request
        // For every authorized request use following header: "authorization" and set value to "Bearer your-token-here"
        private async Task<string> GenerateJwtToken(string email, IdentityUser user, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id), 
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
