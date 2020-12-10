using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Media.Imaging;

namespace ImageServer
{
    public class ImageReceiver
    {
        public event EventHandler<byte[]> onNewImage;
        public void Connect(int Port)
        {
            TcpListener server = new(IPAddress.Any, Port);
            server.Start();
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            byte red = (byte)stream.ReadByte();
            byte green = (byte)stream.ReadByte();
            byte blue = (byte)stream.ReadByte();
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            int size = BitConverter.ToInt32(buffer);
            byte[] imageData = new byte[size];
            stream.Read(imageData, 0, size);
            byte[] newImage = TransformImage(red, green, blue, imageData);
            stream.Write(BitConverter.GetBytes(newImage.Length));
            stream.Write(newImage);
        }

        private byte[] TransformImage(byte red, byte green, byte blue, byte[] image)
        {
            onNewImage.Invoke(this, image);
            int startOfImageData = BitConverter.ToInt32(image, 10);
            for (int i = startOfImageData; i < image.Length; i++)
            {
                image[i] = (byte)(image[i++] * ((float)blue / 255));
                image[i] = (byte)(image[i++] * ((float)green / 255));
                image[i] = (byte)(image[i++] * ((float)red / 255));

                onNewImage.Invoke(this, image);
            }
            return image;
        }
    }
}
