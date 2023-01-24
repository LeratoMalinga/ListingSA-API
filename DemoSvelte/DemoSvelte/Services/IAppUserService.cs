using DemoSvelte.Models;

namespace DemoSvelte.Services
{
    public interface IAppUserService
    {
        List<AppUser> Get();
        AppUser Get(string Id);
        AppUser Create(AppUser appuser);
        void Update(string id, AppUser appuser);
        void Delete(string id);
    }
}
