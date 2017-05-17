using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ttpim.gamemodule.common
{
    public static class CommonWebController
    {
        public static class DownloadImage
        {
            private static string imageUrl;
            private static Bitmap bitmap;

            public static void Download(string _imageUrl)
            {
                imageUrl = _imageUrl;
                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(imageUrl);
                    bitmap = new Bitmap(stream);
                    stream.Flush();
                    stream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            public static Bitmap GetImage()
            {
                return bitmap;
            }
            public static void SaveImage(string filename, ImageFormat format)
            {
                if (bitmap != null)
                {
                    bitmap.Save(filename, format);
                }
            }
        }

        public static MemoryStream CopyToMemory(Stream input)
        {
            MemoryStream ret = new MemoryStream();

            byte[] buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ret.Write(buffer, 0, bytesRead);
            }
            ret.Position = 0;
            return ret;
        }

        public static Image GetLocalImage(string filePath)
        {
            FileStream f = new FileStream(filePath, FileMode.Open);
            MemoryStream ret = new MemoryStream();
            ret = CopyToMemory(f);
            f.Close();
            return Image.FromStream(ret);
        }

        public static Image GetRemoteImage(string filePath)
        {
            WebClient l_WebClient = new WebClient();
            byte[] l_imageBytes = l_WebClient.DownloadData(filePath);
            MemoryStream l_stream = new MemoryStream(l_imageBytes);
            return Image.FromStream(l_stream);
        }

        /// <summary>
        /// Returns the content of a given web adress as string.
        /// </summary>
        /// <param name="Url">URL of the webpage</param>
        /// <returns>Website content</returns>
        public static string DownloadWebPage(string Url)
        {
            // Open a connection
            HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);

            // You can also specify additional header values like 
            // the user agent or the referer:
            //WebRequestObject.UserAgent = ".NET Framework/2.0";
            //WebRequestObject.Referer = "//http:///www.example.com/";

            // Request response:
            WebResponse Response = WebRequestObject.GetResponse();

            // Open data stream:
            Stream WebStream = Response.GetResponseStream();

            // Create reader object:
            StreamReader Reader = new StreamReader(WebStream);

            // Read the entire stream content:
            string PageContent = Reader.ReadToEnd();

            // Cleanup
            Reader.Close();
            WebStream.Close();
            Response.Close();

            return PageContent;
        }

        public static byte[] ConvertImageToBytes(Image img)
        {
            byte[] bytes;
            ImageConverter convertor = new ImageConverter();
            bytes = (byte[])convertor.ConvertTo(img, typeof(byte[]));
            return bytes;
        }
    }
}
