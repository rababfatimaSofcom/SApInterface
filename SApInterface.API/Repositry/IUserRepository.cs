using SApInterface.API.Model.Domain;

namespace SApInterface.API.Repositry
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
