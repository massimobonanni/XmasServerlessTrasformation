using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using SCP.Client.Messages;

namespace SCP.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindow_Closing;
            Messenger.Default.Register<UserMessage>(this, UserMessageReceived);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Messenger.Default.Unregister<UserMessage>(this, UserMessageReceived);
        }

        private void UserMessageReceived(UserMessage message)
        {
            MessageBoxImage messageIcon = MessageBoxImage.None;
            switch (message.MessageType)
            {
                case UserMessage.Type.Error:
                    messageIcon = MessageBoxImage.Error;
                    break;
                case UserMessage.Type.Info:
                    messageIcon = MessageBoxImage.Information;
                    break;
                case UserMessage.Type.Warning:
                    messageIcon = MessageBoxImage.Warning;
                    break;
            }

            MessageBox.Show(message.Message, message.Title, MessageBoxButton.OK, messageIcon);
        }


    }
}
