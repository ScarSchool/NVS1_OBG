using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageClient
{
    public static class ImageUploader
    {
        public static byte[] UploadAndReceive(string url, int port, byte red, byte green, byte blue, byte[] image)
        {
            using NetworkStream stream = new TcpClient(url, port).GetStream();
            byte[] metaBuffer = new byte[] { red,green,blue };
            stream.Write(metaBuffer);
            stream.Write(BitConverter.GetBytes(image.Length));
            stream.Write(image);
            byte[] imgLength = new byte[4];
            stream.Read(imgLength, 0, 4);
            int imgSize = BitConverter.ToInt32(imgLength);
            byte[] img = new byte[imgSize];
            stream.Read(img, 0, imgSize);
            return img;
        }
    }
}
