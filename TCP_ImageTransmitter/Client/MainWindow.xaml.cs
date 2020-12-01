using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Client
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;

        public MainWindow()
        {
            InitializeComponent();
            Main();
        }

        private void Main()
        {
            this.Closing += MainWindow_Closing;

            client = new TcpClient("localhost", 1337);
            stream = client.GetStream();
            ReadImage(stream);
        }

        /// <summary>
        /// Closes the stream and the client connection once the window gets closed / exited. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stream?.Close();
            client?.Close();
        }

        /// <summary>
        /// Reads an Image from a Networkstream
        /// </summary>
        /// <param name="stream">The TPC client connections stream</param>
        private void ReadImage(NetworkStream stream)
        {
            byte[] imageSizeBuffer = new byte[4];
            
            ReadAsync(stream, imageSizeBuffer, (_) =>
            {
                int imageSize = BitConverter.ToInt32(imageSizeBuffer, 0);
                byte[] imageData = new byte[imageSize];
                
                ReadAsync(stream, imageData, (__) =>
                {
                    Dispatcher.Invoke(() => {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = new MemoryStream(imageData);
                        image.EndInit();

                        imageViewer.Source = image;
                    });

                    ReadImage(stream);
                });
            });
        }


        private void ReadAsync(NetworkStream stream, byte[] buffer, Action<int> callback)
        {
            stream.BeginRead(buffer, 0, buffer.Length, ar => {
                try
                {
                    int readBytes = stream.EndRead(ar);
                    callback(readBytes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Got an Exception while reading the data", MessageBoxButton.YesNoCancel);
                }
            }, stream);
        }


    }
}
