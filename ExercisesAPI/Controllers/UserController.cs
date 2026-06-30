using ExercisesAPI.Auth;
using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExercisesAPI.Controllers
{
    public class HashSalt
    {
        public string? Hash { get; set; }
        public string? Salt { get; set; }
    }

    //[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        readonly AppDbContext? _ctx;
        readonly IConfiguration configuration;
        public UserController(AppDbContext context, IConfiguration config) // injected here
        {
            _ctx = context;
            this.configuration = config;
        }

        [HttpPost]
        [Route("api/[controller]/Register")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<ActionResult<UserAuth>> Register(UserAuth auth)
        {
            UserDAO dao = new(_ctx!);
            User? already = await dao.GetByEmail(auth.Email);
            if (already == null)
            {
                HashSalt hashSalt = GenerateSaltedHash(64, auth.Password!);
                auth.Password = ""; // don’t need the string anymore
                User newUser = new()
                {
                    FirstName = auth.FirstName!,
                    LastName = auth.LastName!,
                    Email = auth.Email!,
                    Hash = hashSalt.Hash!,
                    Salt = hashSalt.Salt!
                };
                newUser = await dao.Register(newUser);
                if (newUser.Id > 0)
                    auth.Token = "User registered with ID " + newUser.Id;
                else
                    auth.Error = "User registration failed";
            }
            else
            {
                auth.Error = "user registration failed - email already in use";
            }
            return auth;
        }
        private static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = RandomNumberGenerator.Create();
            // Fills an array of bytes with a cryptographically strong sequence of random nonzero values.
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            // a password, salt, and iteration count, then generates a binary key
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            HashSalt hashSalt = new() { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        public static bool VerifyPassword(string? enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword!, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }

        [HttpPost]
        [Route("api/[controller]/Login")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<ActionResult<UserAuth>> Login(UserAuth auth)
        {
            UserDAO dao = new(_ctx!);
            User? user = await dao.GetByEmail(auth.Email);
            if (user != null)
            {
                if (VerifyPassword(auth.Password, user.Hash!, user.Salt!))
                {
                    auth.Password = "";
                    var appSettings = configuration.GetSection("AppSettings").GetValue<string>("Secret");
                    // authentication successful so generate jwt token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(appSettings);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, user.Id.ToString())]),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    string returnToken = tokenHandler.WriteToken(token);
                    auth.Token = returnToken;
                }
                else
                {
                    auth.Error = "username or password invalid - login failed";
                }
            }
            else
            {
                auth.Error = "no such user - login failed";
            }
            return auth;
        }
    }
}
