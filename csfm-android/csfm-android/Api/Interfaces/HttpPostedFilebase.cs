
using Android.Database;
using Android.Provider;
using csfm_android.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace csfm_android.Api.Interfaces
{
    public class HttpPostedFilebase
    {
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        [JsonConverter(typeof(MemoryStreamJsonConverter))]
        public Stream InputStream { get; set; }

        public HttpPostedFilebase(string username, Android.Net.Uri uri)
        {
            byte[] bytes = GetBytes(uri);
            InputStream = new MemoryStream(bytes);
            FileName = username;
            ContentLength = bytes.Length;
            ContentType = CSFMApplication.Context.ContentResolver.GetType(uri);
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>()
            {
                { nameof(InputStream), InputStream },
                { nameof(FileName), FileName },
                { nameof(ContentLength), ContentLength },
                { nameof(ContentType), ContentType }
            };
        }

        //public HttpPostedFilebase(string username, Stream stream)
        //{
        //    InputStream = stream;
        //    FileName = username;
        //    ContentType = "image/png";
        //    ContentLength = (int)stream.Length;
        //}

        private byte[] GetBytes(Android.Net.Uri uri)
        {
            return GetBytes(CSFMApplication.Context.ContentResolver.OpenInputStream(uri));
        }

        private byte[] GetBytes(Stream stream)
        {
            Java.IO.ByteArrayOutputStream byteBuffer = new Java.IO.ByteArrayOutputStream();
            int bufferSize = 30000;
            byte[] buffer = new byte[bufferSize];

            int len = 0;
            while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                byteBuffer.Write(buffer, 0, len);
            }
            return byteBuffer.ToByteArray();
        }

        public string GetPath(Android.Net.Uri uri)
        {
            string[] projection = { MediaStore.Images.Media.InterfaceConsts.Data };
            //ICursor cursor = Activity.ManagedQuery(uri, projection, null, null, null);
            ICursor cursor = CSFMApplication.Context.ContentResolver.Query(uri, projection, null, null, null);
            int columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
            cursor.MoveToFirst();

            return cursor.GetString(columnIndex);
        }
    }

}