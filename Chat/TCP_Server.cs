using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Windows.Threading;

namespace SimpleChatServer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] _buffer = new byte[1024];
        private static MainWindow mainwin;
        private static Socket clientSocket;  
        public MainWindow()
        {
            InitializeComponent();
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 9500));
            serverSocket.Listen(1);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            mainwin = this;
            
        }
        private static void AcceptCallback(IAsyncResult AR)
        {
            clientSocket = serverSocket.EndAccept(AR);  //Jetzt wird ein neuer Socket als  Verbindung zum Client hergestellt
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { mainwin.textBox.Text = mainwin.textBox.Text + "\n" + "Client hat sich verbunden"; });
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);
            string text = Encoding.ASCII.GetString(dataBuffer);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { mainwin.textBox.Text = mainwin.textBox.Text+"\n"+text; });
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(textBox1.Text);
            clientSocket.Send(buffer);
        }
    }
}
