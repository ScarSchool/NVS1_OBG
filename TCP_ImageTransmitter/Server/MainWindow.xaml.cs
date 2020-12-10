using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Server
{
    public partial class MainWindow : Window
    {
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private byte[] imageBytes = new byte[0];

        public MainWindow()
        {
            InitializeComponent();
            Main();
        }

        private void Main()
        { 
            this.Closing += MainWindow_Closing;

            server = new TcpListener(IPAddress.Any, 1337);
            server.Start();
            ConnectClient(server);
        }

        /// <summary>
        /// Stops the server once the window gets closed / exited.
        /// </summary>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            server?.Stop();
        }

        /// <summary>
        /// Activates once you click on the "Select file" button.
        /// Makes you choose a *.jpg | *.png file to send to your clients
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "PNG files (*.png)|*.png| JPEG files (*.jpg)|*.jpg";
            fileDialog.Multiselect = false;

            // You can maybe set these "Special Folders" somewhere but I care too little to find out how
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);

            if (fileDialog.ShowDialog() == true)
            {
                string imagePath = fileDialog.FileName;
                imageBytes = File.ReadAllBytes(imagePath);

                // This pretty much just creates the source for the image
                BitmapImage BMImage = new BitmapImage();
                BMImage.BeginInit();
                BMImage.StreamSource = new MemoryStream(imageBytes);
                BMImage.EndInit();

                imageView.Source = BMImage;

                // Make sure to send the beautiful image you chose to all your clients
                clients.ForEach(client => SendImage(client, imageBytes));
            }
        }

        /// <summary>
        /// Creates a TCP connection to accept TCP clients and adds them to the 'clients' array
        /// </summary>
        /// <param name="server">The TCP Listener you want your clients to be able to connect to</param>
        private void ConnectClient(TcpListener server)
        {
            server.BeginAcceptTcpClient(ar => {
                try
                {
                    TcpClient client = server.EndAcceptTcpClient(ar);
                    clients.Add(client);

                    if (imageBytes.Length > 0) SendImage(client, imageBytes);
                    ConnectClient(server);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Got an Exception while connecting client", MessageBoxButton.YesNoCancel);
                }
            }, server);
        }

        /// <summary>
        /// Sends the imageByteArray to the client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="imageData"></param>
        private void SendImage(TcpClient client, byte[] imageData)
        {
            NetworkStream stream = client.GetStream();
            stream.Write(BitConverter.GetBytes(imageData.Length), 0, 4);
            stream.Write(imageData, 0, imageData.Length);
        }
    }
}
