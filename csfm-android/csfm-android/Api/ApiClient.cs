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
using csfm_android.Api.Model;

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
        
        public string RetrieveUsername()
        {
            return CSFMPrefs.Prefs.GetString(CSFMApplication.Username, "");
        }

        public void Provide(string key, string value)
        {
            CSFMPrefs.Editor.PutString(key, value).Commit();
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
                Provide(CSFMApplication.BearerToken, token);
                Provide(CSFMApplication.Username, username);
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

        public async System.Threading.Tasks.Task<History> GetHistory(string username)
        {
            try
            {
                return await instance.GetUserHistory(username);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task<User> GetUser(string username)
        {
            try
            {
                return await instance.GetUser(username, "Bearer " + this.RetrieveBearer());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task<bool> PutUserLocation(string username, double latitude, double longitude)
        {
            try
            {
                JObject jo = new JObject(
                    new JProperty("latitude", latitude),
                    new JProperty("longitude", longitude));

                await instance.PutUserLocation(username, jo.ToString(), "Bearer " + this.RetrieveBearer());

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async void ImportLastFm(string lastfmUsername)
        {

        }
    }
}
 