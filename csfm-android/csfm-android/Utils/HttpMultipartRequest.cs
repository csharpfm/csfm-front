using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Java.Lang;
using Org.Apache.Http;

namespace csfm_android.Utils
{
    public class HttpMultipartRequest
    {
        const string BOUNDARY = "----------V2ymHFg03ehbqgZCaKO6jy";

        public byte[] postBytes { get; set; }
        string url = null;

        public HttpMultipartRequest(string url, Dictionary<string, string> params_, string fileField, string fileName, string fileType, byte[] fileBytes)
        {
            this.url = url;
            string boundary = GetBoundaryString();
            string boundaryMessage = GetBoundaryMessage(boundary, params_, fileField, fileName, fileType);
            string endBoundary = "\r\n--" + boundary + "--\r\n";
            MemoryStream stream = new MemoryStream();
            var boundaryMessageBytes = GetBytes(boundaryMessage);
            stream.Write(boundaryMessageBytes, 0, boundaryMessageBytes.Length);
            stream.Write(fileBytes, 0, fileBytes.Length);
            var endBBytes = GetBytes(endBoundary);
            stream.Write(endBBytes, 0, endBBytes.Length);
            this.postBytes = stream.ToArray();
            stream.Close();
            
        }

        private string GetBoundaryMessage(string boundary, Dictionary<string, string> params_, string fileField, string fileName, string fileType)
        {
            var res = new StringBuffer("--").Append(boundary).Append("\r\n");

            if (params_ != null)
            {
                var keys = params_.Keys;
                foreach (var k in keys)
                {
                    res.Append("Content-Disposition: form-data; name=\"").Append(k).Append("\"\r\n").Append("\r\n").Append(params_[k]).Append("\r\n").Append("--").Append(boundary).Append("\r\n");
                }
            }
            
            res.Append("Content-Disposition: form-data; name=\"").Append(fileField).Append("\"; filename=\"").Append(fileName).Append("\"\r\n").Append("Content-Type: ").Append(fileType).Append("\r\n\r\n");

            return res.ToString();
        }

        private string GetBoundaryString()
        {
            return BOUNDARY;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}