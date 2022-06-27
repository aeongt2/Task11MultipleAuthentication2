using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Task11MultipleAuthentication2.Data;
using Task11MultipleAuthentication2.Models;
using Task11MultipleAuthentication2.Models.VMs;
using Task11MultipleAuthentication2.Services.IServices;

namespace Task11MultipleAuthentication2.Services
{
    public class AccountService : IAccountService
    {
        public readonly ApplicationDbContext _dbContext;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = db;
            _configuration = configuration;
        }

        public async Task<JWTLoginVM> LoginWithJWT(LoginVM model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            var loginTest = await _userManager.CheckPasswordAsync(user, model.Password);
            if (loginTest)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                var result = new JWTLoginVM()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Validity = token.ValidTo.ToString()
                };
                var date = DateTime.Now;
                return result;
            }
            return null;
        }
        public async Task<bool> Login(LoginVM loginVM)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return true;
            }
            return false;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }


        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> RegisterNewUser(RegisterVM registerVM)
        {
            var _user = new ApplicationUser
            {
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                UserName = registerVM.Email
            };
            var result = _userManager.CreateAsync(_user, registerVM.Password).Result;
            if (!result.Succeeded)
            {
                string output = string.Empty;
                foreach (var item in result.Errors)
                {
                    output = output + item.Description;
                }
                return output;
            }
            return null;
        }
    }
}
