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
using Refit;
using csfm_android.Api.Interfaces;
using csfm_android.Utils;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Globalization;
using Newtonsoft.Json;
using Android.Graphics;
using Android.Provider;

namespace csfm_android.Api
{
    public class ApiClient
    {
        private const string SERVER_URL = "http://matchfm.westeurope.cloudapp.azure.com";
        //private const string SERVER_URL = "http://matchfm.azurewebsites.net";

        private static readonly ICsfmApi instance = RestService.For<ICsfmApi>(SERVER_URL);

        public string Bearer
        {
            get
            {
                return "Bearer " + this.RetrieveBearer();
            }
        }

        public ApiClient()
        {
        }


        public string RetrieveBearer()
        {
            return CSFMPrefs.Prefs.GetString(CSFMApplication.BearerToken, "");
        }

        public void ProvideBearer(string bearer)
        {
            var editor = CSFMPrefs.Editor;
            editor.PutString(CSFMApplication.BearerToken, bearer);
            editor.Commit();
        }

        public async System.Threading.Tasks.Task<bool> LogIn(string username, string password)
        {
            Dictionary<String, object> informations = new Dictionary<String, object>();
            informations.Add("grant_type", "password");
            informations.Add("username", username);
            informations.Add("password", password);

            try
            {
                var response = await instance.SignIn(informations);
                var token = JObject.Parse(response)["access_token"].ToString();
                ProvideBearer(token);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async System.Threading.Tasks.Task<bool> SignUp(string email, string username, string password)
        {
            Dictionary<String, object> informations = new Dictionary<String, object>();
            informations.Add("Username", username);
            informations.Add("Email", email);
            informations.Add("Password", password);

            try
            {
                var status = await instance.SignUp(informations);

                if (String.IsNullOrEmpty(status))
                {
                    return true;
                }
            }
            catch (Refit.ApiException e)
            {
                return false;
            }

            return false;
        }

        public async System.Threading.Tasks.Task<bool> GetUser(string username)
        {
            var user = await instance.GetUser(username, "Bearer " + this.RetrieveBearer());
            // TODO
            return true; // TODO
        }

        public async Task UploadProfilePicture(string username, HttpPostedFilebase aFile, string path, byte[] bytes)
        {

            string lineEnd = "\r\n";
            string twoHyphens = "--";
            string boundary = "*****";
            //try
            //{

            //    String lineEnd = "\r\n";
            //    String twoHyphens = "--";
            //    String boundary = "*****";
            //    int maxBufferSize = 1 * 1024 * 1024;
            //    Java.IO.FileInputStream fileInputStream = new Java.IO.FileInputStream(new Java.IO.File(path));
            //    Java.Net.URL url = new Java.Net.URL(SERVER_URL + "/api/Users/" + username + "/Photo");
            //    Java.Net.HttpURLConnection conn = (Java.Net.HttpURLConnection)url.OpenConnection();
            //    conn.DoInput = true;
            //    conn.DoOutput = true;
            //    conn.UseCaches = false;
            //    conn.RequestMethod = "POST";
            //    conn.SetRequestProperty("Connection", "Keep-Alive");
            //    conn.SetRequestProperty("Authorization", this.Bearer);
            //    conn.SetRequestProperty("Content-Type", "application/octet-stream");
            //    conn.SetRequestProperty("uploaded_file", username);

            //    var dos = new Java.IO.DataOutputStream(conn.OutputStream);
            //    dos.WriteBytes(twoHyphens + boundary + lineEnd);
            //    dos.WriteBytes("Content-Disposition: form-data; name=\"uploaded_file\"; filename=\"" + username + "\"" + lineEnd);
            //    dos.WriteBytes(lineEnd);

            //    var bytesAvailable = fileInputStream.Available();

            //    var bufferSize = Math.Min(bytesAvailable, maxBufferSize);
            //    var buffer = new byte[bufferSize];
            //    var bytesRead = fileInputStream.Read(buffer, 0, bufferSize);

            //    while (bytesRead > 0)
            //    {
            //        dos.Write(buffer, 0, bufferSize);
            //        bytesAvailable = fileInputStream.Available();
            //        bufferSize = Math.Min(bytesAvailable, maxBufferSize);
            //        bytesRead = fileInputStream.Read(buffer, 0, bufferSize);
            //    }

            //    //Send multipart form data necessary after file data
            //    dos.WriteBytes(lineEnd);
            //    dos.WriteBytes(twoHyphens + boundary + twoHyphens + lineEnd);

            //    //Response from the server (code and message)
            //    var serverResponseCode = conn.ResponseCode;
            //    var serverResponseMessage = conn.ResponseMessage;

            //    Console.WriteLine("Done");

            //    fileInputStream.Close();
            //    dos.Flush();
            //    dos.Close();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}






            var url = SERVER_URL + "/api/Users/" + username + "/Photo";

            try
            {
                Uri uri = new Uri(url);

                WebClient client = new WebClient();
                client.Headers = new WebHeaderCollection();
                client.Headers[HttpRequestHeader.Authorization] = this.Bearer;
                //client.Headers[HttpRequestHeader.ContentType] = "multipart/form-data";

                try
                {
                    //byte[] result = client.UploadFile(uri, "POST", path);
                    //HttpMultipartRequest req = new HttpMultipartRequest(url, null, "upload_field", "original_filename.png", "image/png", bytes);
                    client.UploadData(uri, "POST", bytes);
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex);
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                    object obj = JsonConvert.DeserializeObject(resp);
                    Console.WriteLine("test");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            //try
            //{
            //    Console.WriteLine("API START ---------------------");
            //    string result = await instance.UploadPhoto(username, bytes, this.Bearer);
            //    Console.WriteLine(result);

            //}
            //catch (Refit.ApiException e)
            //{
            //    Console.WriteLine("---------- Exception ------------");
            //    Console.WriteLine(e);
            //    var content = e.GetContentAs<System.Collections.Generic.Dictionary<string, string>>();
            //    foreach (var item in content)
            //    {
            //        Console.WriteLine(item.Key + ":" + item.Value);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Unknown Exception");
            //    Console.WriteLine(e);
            //}
        }


    }


}
 