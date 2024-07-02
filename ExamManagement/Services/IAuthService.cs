using EasyTestMaker.Models;
using System.Threading.Tasks;

namespace EasyTestMaker.Services
{
    public interface IAuthService
    {
        Task<bool> Login(User user);
        Task<bool> CreateUser(User user);
    }
}