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
using System.Net;

namespace csfm_android.Api
{
    public sealed class ApiClient
    {
        private static readonly string SERVER_URL = "";
        private static readonly ApiClient instance = new ApiClient();
        private static readonly Uri endpoint = new Uri(SERVER_URL);
        private string bearer;
        private bool authenticated;

        public static ApiClient Instance
        {
            get
            {
                return instance;
            }
        }

        private ApiClient()
        {
        }


        public bool SignUp(string username, string password)
        {/*
            Uri uri = new Uri(endpoint, "/api/Account/Register");


            HttpContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", username),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("ConfirmPassword", password),
            });

            HttpResponseMessage result = (new HttpClient()).PostAsync(uri, content).Result;

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;*/
            return false;
        }

        public string RetrieveBearer()
        {
            return this.bearer;
        }

        public void ProvideBearer(string bearer)
        {
            this.bearer = bearer;
        }

        public bool LogIn(string username, string password)
        {
            /*   Uri uri = new Uri(endpoint, "/oauth/token");
               HttpContext content = new FormUrlEncodedContent(new[]
               {
                   new KeyValuePair<string, string>("username", username),
                   new KeyValuePair<string, string>("password", password),
                   new KeyValuePair<string, string>("grant_type", "password")
               });

               HttpResponseMessage result = (new HttpClient()).PostAsync(uri, content).Result;
               if (!result.IsSuccessStatusCode)
               {
                   return false;
               }

               Dictionary<string, string> data =
                   JsonConvert.DeserializeObject<Dictionary<string, string>>(result.Content.ReadAsStringAsync().Result);

               ProvideBearer(data["access_token"]);
               return true;*/
            return false;
        }

    }
}