using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat.Client
{
    public static class Message
    {
        public static void ShowErrorMessage(string errorMessage)
           => MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ShowMessage(string message)
            => MessageBox.Show(message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
