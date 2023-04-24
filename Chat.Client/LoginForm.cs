using Chat.Client.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat.Client
{
    public partial class LoginForm : Form, ILoginView
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public event Action<string, string> LogIn;

        public void HideForm() => Hide();

        public void HideLoader()
        {
            loginButton.Enabled = true;
        }

        public Task ShowLoader()
        {
            loginButton.Enabled = false;
            return Task.CompletedTask;
        }

        DialogResult IView.Show()
        {
            Application.Run(this);
            return DialogResult.None;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            LogIn?.Invoke(textBox1.Text, textBox2.Text);
        }
    }
}
