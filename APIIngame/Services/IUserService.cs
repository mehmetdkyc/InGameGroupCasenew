using APIIngame.Models;
using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace APIIngame.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> ChangePasswordAsync(ChangePasswordModel model);
        Task<UserManagerResponse> RegisterUserAsync(RegisterModel model);

        Task<UserManagerResponse> ForgotPasswordAsync(string email);
        Task<UserManagerResponse> LoginUserAsync(LoginModel model);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        public UserService(Context context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<UserManagerResponse> ChangePasswordAsync(ChangePasswordModel model)
        {
            if (model == null)
                throw new NullReferenceException("Change Password Model is null");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    Message = "User not found.",
                    IsSuccess = false
                };
            var result = await _userManager.CheckPasswordAsync(user, model.OldPasword);
            if (!result)
            {
                return new UserManagerResponse()
                {
                    Message = "Invalid Old Password",
                    IsSuccess = false
                };
            }
            var changedResult = await _userManager.ChangePasswordAsync(user, model.OldPasword, model.NewPassword);
            return new UserManagerResponse()
            {
                Message = "Password has been changed successfully.",
                IsSuccess = true
            };
        }

        public async Task<UserManagerResponse> ForgotPasswordAsync(string email)
        {
            if (email == null)
                throw new NullReferenceException("email is null");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new UserManagerResponse
                {
                    Message = "User not found.",
                    IsSuccess = false
                };
            if (SendNewPasswordToMail(email))
            {
                return new UserManagerResponse()
                {
                    Message = "New password sent to email",
                    IsSuccess = true
                };
            }
            return new UserManagerResponse()
            {
                Message = "Something went wrong",
                IsSuccess = false
            };
        }

        private bool SendNewPasswordToMail(string email)
        {
            byte[] rgb = new byte[10];
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            rngCrypt.GetBytes(rgb);
            var newPassword = Convert.ToBase64String(rgb);

            using SmtpClient emailClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(email, newPassword)
            };
            string subject = "Test password";
            string body = $"this is the main email sent @ {DateTime.UtcNow:F}";
            emailClient.Send("mehmetdokuyucu98@gmail.com", email, subject, body);
            try
            {
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse()
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new UserManagerResponse()
                {
                    Message = "Invalid Password",
                    IsSuccess = false
                };
            }
            var claims = new[]
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);


            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterModel registerModel)
        {
            if (registerModel == null)
                throw new NullReferenceException("Register Model is null");

            if (registerModel.Password != registerModel.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password.",
                    IsSuccess = false
                };
            IdentityUser user = new IdentityUser()
            {
                Email = registerModel.EmailAddress,
                UserName = registerModel.EmailAddress

            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return new UserManagerResponse()
                {
                    Message = "User created successfuly",
                    IsSuccess = true
                };
            }
            return new UserManagerResponse()
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}
