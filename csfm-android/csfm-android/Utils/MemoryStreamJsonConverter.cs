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
using Newtonsoft.Json;

namespace csfm_android.Utils
{
    public class MemoryStreamJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MemoryStream).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var bytes = serializer.Deserialize<byte[]>(reader);
            return bytes != null ? new MemoryStream(bytes) : new MemoryStream();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bytes = ((MemoryStream)value).ToArray();
            serializer.Serialize(writer, bytes);
        }
    }

    //public sealed class MemoryHttpPostedFilebase : HttpPostedFileBase
    //{
    //    readonly string contentType;
    //    readonly string fileName;
    //    readonly MemoryStream inputStream;

    //    public MemoryHttpPostedFileBase(string contentType, string fileName, [JsonConverter(typeof(MemoryStreamJsonConverter))] MemoryStream inputStream)
    //    {
    //        if (inputStream == null)
    //            throw new ArgumentNullException("inputStream");
    //        this.contentType = contentType;
    //        this.fileName = fileName;
    //        this.inputStream = inputStream;
    //    }

    //    public override int ContentLength
    //    {
    //        get
    //        {
    //            return (int)inputStream.Length;
    //        }
    //    }

    //    public override string ContentType
    //    {
    //        get
    //        {
    //            return contentType;
    //        }
    //    }

    //    public override string FileName
    //    {
    //        get
    //        {
    //            return fileName;
    //        }
    //    }

    //    [JsonConverter(typeof(MemoryStreamJsonConverter))]
    //    public override Stream InputStream
    //    {
    //        get
    //        {
    //            return inputStream;
    //        }
    //    }
    //}

}