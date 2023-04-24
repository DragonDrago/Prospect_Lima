using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.View
{
    public interface ILoginView : IView
    {
        event Action<string, string> LogIn;
        Task ShowLoader();
        void HideLoader();
        void HideForm();
        void Close();
    }
}
