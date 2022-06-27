using System.Threading.Tasks;
using Task11MultipleAuthentication2.Models.VMs;

namespace Task11MultipleAuthentication2.Services.IServices
{
    public interface IAccountService
    {
        public Task<string> RegisterNewUser(RegisterVM registerVM);
        public Task<JWTLoginVM> LoginWithJWT(LoginVM loginVM);
        public Task<bool> Login(LoginVM loginVM);
        public Task Logout();
    }
}
