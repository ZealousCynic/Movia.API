using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Services
{
    public class AccountService
    {
        private readonly IConfiguration _Configuration;
        private readonly IUserRepository _Repository;
        private readonly IRoleRepository _RoleRepository;
        private readonly AuthPolicyRequirement _AuthPolicyRequirement;

        public AccountService(IUserRepository userRepository, IConfiguration configuration, IRoleRepository roleRepository, AuthPolicyRequirement authPolicyRequirement)
        {
            this._Repository = userRepository;
            this._Configuration = configuration;
            this._RoleRepository = roleRepository;
            this._AuthPolicyRequirement = authPolicyRequirement;
        }

        public User GetUserInfo(string email, string password)
        {
            var user = this._Repository.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return null;
            }

            return user;
        }
        public string GetToken(string email, string password)
        {
            var user = this._Repository.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return null;
            }

            //Get signing secret key
            var authSigningKey = Encoding.ASCII.GetBytes(this._Configuration["TokenSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(int.Parse(this._Configuration["TokenSettings:HoursExpires"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(authSigningKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //datetime of expiration
            //expiration = token.ValidTo,

            this.CreatePermissionList();

            return tokenHandler.WriteToken(token);
        }

        public string GetTokenPayload(IEnumerable<Claim> claims, string typeOfClaim)
        {
            var claim = claims.First(c => c.Type == typeOfClaim);
            if (claim == null)
            {
                return null;
            }
            return claim.Value;
        }

        public void CreatePermissionList()
        {
            this._AuthPolicyRequirement.RolePermissions = this._RoleRepository.Roles.ToList();
        }
    }
}
