using Chat.Client.Domain;
using Chat.Client.View;
using Microsoft.AspNetCore.SignalR.Client;
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
    public partial class ChatForm : Form, IChatView
    {
        private HubConnection connection;

        public event Action Loaded;
        public event Action<ChatItem> ChatSelected;
        public event Action<string> SendMessage;
        public event Action WindowClosed;

        public ChatForm()
        {
            InitializeComponent();

            listBox2.Visible = richTextBox1.Visible = sendMessageButton.Visible = false;

            Load += (o, e) => Loaded?.Invoke();
        }

        public void ReceiveMessage(string message)
        {
            listBox2.Items.Add(message);
        }

        DialogResult IView.Show() => ShowDialog();

        public void SetChats(IEnumerable<ChatItem> chatItems)
        {
            chatsListBox.Items.AddRange(chatItems.ToArray());
        }

        private void chatsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Visible = richTextBox1.Visible = sendMessageButton.Visible = true;

            listBox2.Items.Clear();
            richTextBox1.Text = String.Empty;

            ChatItem selectedChatItem = chatsListBox.SelectedItem as ChatItem;

            label1.Text = selectedChatItem.ToString();

            ChatSelected?.Invoke(selectedChatItem);
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            SendMessage?.Invoke(richTextBox1.Text);

            richTextBox1.Text = String.Empty;
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            WindowClosed?.Invoke();
        }

        public void Notify(int userId)
        {
       
        }

        public void SetChatMessages(IEnumerable<ChatMessage> chatMessages)
        {
            listBox2.Items.AddRange(chatMessages.ToArray());
        }
    }
}
