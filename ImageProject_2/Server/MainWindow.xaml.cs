using ImageClient;
using Microsoft.Win32;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double red;
        private double green;
        private double blue;
        private byte[] img;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            var newSource = await Task.Run(() =>
            {
                if (img.Length > 0)
                { 
                    return ImageUploader.UploadAndReceive("localhost", 8000, (byte)red,(byte)green, (byte)blue, img);
                }
                throw new Exception("No Image");
            });

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(newSource);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                afterImage.Source = biImg;
            });
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog imageLoadDialog = new();
            imageLoadDialog.Filter = "PNG files (*.png)|*.png";
            imageLoadDialog.Multiselect = false;
            imageLoadDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (imageLoadDialog.ShowDialog() == true)
            {
                string imageUrl = imageLoadDialog.FileName;
                System.Drawing.Image test = System.Drawing.Image.FromFile(imageUrl);
                using MemoryStream stream = new();
                test.Save(stream, ImageFormat.Bmp);
                img = stream.GetBuffer();
                imgPreview.Source = ByteToImage(img);
            }
        }

        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            return biImg;
        }

        private void sliderBlue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            blue = e.NewValue;
        }
        private void sliderRed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            red = e.NewValue;
        }

        private void sliderGreen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            green = e.NewValue;
        }
    }
}
