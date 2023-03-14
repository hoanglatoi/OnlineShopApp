using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Model.Models;
using OnlineShop.Data;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace OnlineShop.Service.Services.Token
{
    public class TokenManager : ITokenManager
    {
        private readonly ILogger<TokenManager> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationGroupRepository _applicationGroupRepository;
        private readonly IStringCompression _stringCompression;
        private readonly IApplicationRoleRepository _applicationRoleRepository;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenManager(IConfiguration configuration, /*IUserService userService,*/ 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<TokenManager> logger,
            IUnitOfWork unitOfWork,
            IApplicationGroupRepository applicationGroupRepository,
            IApplicationRoleRepository applicationRoleRepository,
            IStringCompression stringCompression
            )
        {
            _logger = logger;
            _configuration = configuration;
            //_userService = userService;

            _signInManager = signInManager;
            _userManager = userManager;

            _unitOfWork = unitOfWork;
            _applicationGroupRepository = applicationGroupRepository;
            _applicationRoleRepository = applicationRoleRepository;
            _stringCompression = stringCompression;
        }

        private string CreateToken(List<Claim> claims)
        {
            string jwtSecurityKey = _configuration.GetSection("Jwt:JwtSecurityKey").Value;
            if(string.IsNullOrEmpty(jwtSecurityKey)) throw new ArgumentException("JwtSecurityKey is not setted in appsettings.json");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecurityKey));

            string algorithm = _configuration.GetSection("TokenSettings:SecurityAlgorithm").Value;
            SigningCredentials creds;
            switch (algorithm)
            {
                case "HmacSha256Signature":
                    creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    break;
                case "HmacSha512Signature":
                    creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    break;
                case "RsaSha256Signature":
                    creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
                    break;
                case "RsaSha512Signature":
                    creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha512Signature);
                    break;
                default: 
                    throw new ArgumentException("SecurityAlgorithm is not setted in appsettings.json");
            }

            DateTime? expires = null;
            string expireUnit = _configuration.GetSection("TokenSettings:ExpireUnit").Value;
            bool ret = int.TryParse(_configuration.GetSection("TokenSettings:Expire").Value, out int expireNumber);
            if(ret == false || string.IsNullOrEmpty(expireUnit))
            {
                expireUnit = "min";
                expireNumber = 30;
            }
            switch (expireUnit)
            {
                case "min":
                    expires = DateTime.Now.AddMinutes(expireNumber);
                    break;
                case "hour":
                    expires = DateTime.Now.AddHours(expireNumber);
                    break;
                case "day":
                    expires = DateTime.Now.AddDays(expireNumber);
                    break;
                case "month":
                    expires = DateTime.Now.AddMonths(expireNumber);
                    break;
            }
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string GenerateAccessToken(ApplicationUser user, ApplicationGroup group)
        {
            ApplicationRole? role = new ApplicationRole();
            if(group.Name == Group.Maintenancer)
            {
                role = _applicationRoleRepository.GetSingleByCondition(x => x.Name == Role.Admin);
                if(role == null)
                {
                    role = _applicationRoleRepository.GetSingleByCondition(x => x.Name == Role.Worker);
                }
            }
            else if(group.Name == Group.Customer)
            {
                role = _applicationRoleRepository.GetSingleByCondition(x => x.Name == Role.VipMember);
                if (role == null)
                {
                    role = _applicationRoleRepository.GetSingleByCondition(x => x.Name == Role.BasicMember);
                }
            }
            else
            {
                return String.Empty;
            }
            if (role == null) return String.Empty;
            var accessToken = new AccessToken(user, role, group);

            // Add Claim
            List<Claim> claims = new List<Claim>();
            foreach (PropertyInfo prop in accessToken.GetType().GetProperties())
            {
                var atributes = prop.GetCustomAttributes(true);
                if (atributes.ToList().Exists(x => x.GetType() == typeof(RequiredAttribute)) == false) continue;
                claims.Add(new Claim(prop.Name.ToString(), prop.GetValue(accessToken, null)?.ToString() ?? ""));
            }

            return CreateToken(claims);
        }


        public RefreshToken GenerateRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken
            {
                UserName = userName,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(1),
                Created = DateTime.Now
            };

            return refreshToken;
        }
    }
}
