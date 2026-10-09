using ModelLayer;
using ModelLayer.Model;
using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using MessageService.Interface;
using AuthenticationService.Interface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;
        private readonly IMessageService _messageService;

        public UserBL(
            IUserRL userRL,
            IJwtService jwtService,
            IMessageService messageService)
        {
            _userRL = userRL;
            _jwtService = jwtService;
            _messageService = messageService;
        }

        public ResponseModel<RegistrationModel> RegisterUserBL(
            RegistrationModel registrationModel)
        {
            RegistrationModel result =
                _userRL.RegisterUserRL(registrationModel);

            return new ResponseModel<RegistrationModel>
            {
                Success = true,
                Message = "User registered successfully",
                Data = result
            };
        }

        public ResponseModel<string> LoginUserBL(
            LoginModel loginModel)
        {
            var user = _userRL.LoginUserRL(loginModel);

            if (user == null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Data = null
                };
            }

            string token = _jwtService.GenerateToken(user);

            return new ResponseModel<string>
            {
                Success = true,
                Message = "Login successful",
                Data = token
            };
        }

        public async Task<ResponseModel<string>> ForgotPasswordBL(
            ForgotPasswordModel model)
        {
            var user = _userRL.GetUserByEmailRL(model.Email);

            if (user == null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }

            string resetToken = _jwtService.GenerateResetToken(user);
            
            string subject = "Fundoo Password Reset";

            string body =
                $"Hello {user.FirstName},\n\n" +
                "You requested to reset your Fundoo password.\n\n" +
                "Use the following token in the Reset Password API:\n\n" +
                $"{resetToken}\n\n" +
                "This token is valid for 10 minutes.\n\n" +
                "If you did not request this password reset, ignore this email.\n\n" +
                "Regards,\n" +
                "Fundoo Team";

            await _messageService.SendEmailAsync(
                user.Email,
                subject,
                body);

            return new ResponseModel<string>
            {
                Success = true,
                Message =
                    "Password reset token sent to your registered email",
                Data = null
            };
        }

        public ResponseModel<string> ResetPasswordBL(
            ResetPasswordModel model)
        {
            try
            {
                int userId = _jwtService.ValidateResetToken(model.Token);

                bool result = _userRL.ResetPasswordRL(
                        userId,
                        model.NewPassword);

                if (!result)
                {
                    return new ResponseModel<string>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }

                return new ResponseModel<string>
                {
                    Success = true,
                    Message = "Password reset successfully",
                    Data = null
                };
            }
            catch
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid or expired reset token",
                    Data = null
                };
            }
        }
    }
}