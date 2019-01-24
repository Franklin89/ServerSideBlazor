using System;

namespace ServerSideBlazor.App.Services
{
    public interface IUserService
    {
        event EventHandler UserChanged;
        void Login();
        void Logout();
    }

    public class UserService : IUserService
    {
        public event EventHandler UserChanged;

        public void Login()
        {
            OnUserChanged();
        }

        public void Logout()
        {
            OnUserChanged();
        }

        protected virtual void OnUserChanged()
        {
            UserChanged?.Invoke(this, new EventArgs());
        }
    }
}