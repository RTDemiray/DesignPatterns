using BaseProject.Models;

namespace BaseProject.Observer
{
    public interface IUserObserver
    {
        void UserCreated(AppUser appUser);
    }
}