using SApInterface.API.Model.Domain;

namespace SApInterface.API.Repositry
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
