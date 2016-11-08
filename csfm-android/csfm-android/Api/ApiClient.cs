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

namespace csfm_android.Api
{
    public class ApiClient
    {
        private static readonly string SERVER_URL = "http://matchfm.azurewebsites.net";

        private static readonly ICsfmApi instance = RestService.For<ICsfmApi>(SERVER_URL);

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

        public async System.Threading.Tasks.Task<string> GetHistory(string username)
        {
            try
            {
                var history = await instance.GetUserHistory(username);
                return history;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public async System.Threading.Tasks.Task<bool> GetUser(string username)
        {
            var user = await instance.GetUser(username, "Bearer " + this.RetrieveBearer());
            // TODO
            return true; // TODO
        }
    }
}
 