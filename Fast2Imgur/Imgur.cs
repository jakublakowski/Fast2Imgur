using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fast2Imgur
{
    public static class Imgur
    {
        //Please dont overuse!
        //You can easily get your's at https://api.imgur.com/oauth2/addclient
        static readonly string clientID = "93bd17bcc75e6c0";

        public static string UploadImage(string imagePath)
        {
            using (var w = new WebClient())
            {
                w.Headers.Add("Authorization", "Client-ID " + clientID);
                var values = new NameValueCollection
                {
                    { "image", Convert.ToBase64String(File.ReadAllBytes(imagePath)) }
                };

                return GetLink(w, values);
            }
        }
        public static string UploadImage(Image image)
        {
            using (var w = new WebClient())
            {
                w.Headers.Add("Authorization", $"Client-ID {clientID}");

                ImageConverter _imageConverter = new ImageConverter();
                byte[] imageByte = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));

                var values = new NameValueCollection
                {
                    { "image", Convert.ToBase64String(imageByte) }
                };

                return GetLink(w, values);
            }
        }

        private static string GetLink(WebClient w, NameValueCollection values)
        {
            
            byte[] response = w.UploadValues("https://api.imgur.com/3/image?_format=xml", values);
            string result = System.Text.Encoding.UTF8.GetString(response);

            var xmlDoc = XDocument.Load(new MemoryStream(response));
            var link = xmlDoc.Root.Elements().Where(x => x.Name == "link").First().Value;
            return link;
        }
    }
}
