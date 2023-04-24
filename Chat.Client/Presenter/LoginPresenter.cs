using Chat.Client.Repository;
using Chat.Client.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat.Client.Presenter
{
    public class LoginPresenter : IPresenter
    {
        private ILoginView _logInView { get; set; }

        public LoginPresenter(
            ILoginView view)
        {
            _logInView = view;

            _logInView.LogIn += async (login, password) => await View_LogIn(login, password);
        }

        private async Task View_LogIn(string login, string password)
        {
            try
            {
                if (!Validate(login, password))
                    return;

                var response = await AppService.AuthenticateRepository.Login(login, password);

                AppGlobal.Account = response;

                _logInView.HideForm();

                ChatPresenter mainPresenter = new ChatPresenter(new ChatForm());
                mainPresenter.Close += () => _logInView.Close();
                mainPresenter.Run();
            }
            catch (Exception ex)
            {
                _logInView.HideLoader();
                Message.ShowErrorMessage(ex.Message);
            }
            finally
            {
                _logInView.HideLoader();
            }
        }

        private bool Validate(string login, string password)
        {
            if (string.IsNullOrEmpty(login?.Trim()))
            {
                Message.ShowErrorMessage("Введите логин.");
                return false;
            }

            if (string.IsNullOrEmpty(password?.Trim()))
            {
                Message.ShowErrorMessage("Введите пароль.");
                return false;
            }

            return true;
        }

        public DialogResult Run() => _logInView.Show();
    }
}
