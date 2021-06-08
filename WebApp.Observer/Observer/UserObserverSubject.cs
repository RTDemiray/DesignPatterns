using System.Collections.Generic;
using BaseProject.Models;

namespace BaseProject.Observer
{
    public class UserObserverSubject
    {
        private readonly List<IUserObserver> _userObservers;

        public UserObserverSubject()
        {
            _userObservers = new();
        }

        public void RegisterObserver(IUserObserver userObserver)
        {
            _userObservers.Add(userObserver);
        }

        public void RemoveObserver(IUserObserver userObserver)
        {
            _userObservers.Remove(userObserver);
        }

        public void NotifyObserver(AppUser appUser)
        {
            _userObservers.ForEach(x =>
            {
                x.UserCreated(appUser);
            });
        }
    }
}