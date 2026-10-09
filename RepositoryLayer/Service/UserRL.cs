using RepositoryLayer.Interface;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using ModelLayer.Model;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        FundooContext fundooContext;

        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public RegistrationModel RegisterUserRL(RegistrationModel registrationModel)
        {
            UserEntity userEntity = new UserEntity();

            userEntity.FirstName = registrationModel.firstName;
            userEntity.LastName = registrationModel.lastName;
            userEntity.Email = registrationModel.email;
            userEntity.Password = registrationModel.password;
            userEntity.PhoneNumber = registrationModel.contactNo;

            fundooContext.Users.Add(userEntity);

            var result = fundooContext.SaveChanges();

            Console.WriteLine("result=" + result);

            return registrationModel;
        }
        public UserEntity? LoginUserRL(LoginModel loginModel)
        {
            var user = fundooContext.Users
                .FirstOrDefault(x =>
                    x.Email == loginModel.email &&
                    x.Password == loginModel.password);

            return user;
        }
        public UserEntity? GetUserByEmailRL(string email)
        {
            return fundooContext.Users
                .FirstOrDefault(x => x.Email == email);
        }

        public bool ResetPasswordRL(int userId, string newPassword)
        {
            var user = fundooContext.Users
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                return false;
            }

            user.Password = newPassword;

            fundooContext.SaveChanges();

            return true;
        }
    }
}