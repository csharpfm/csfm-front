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

namespace csfm_android.Api
{
    public class ApiClient
    {
        private static readonly string SERVER_URL = "";
        private static readonly ICsfmApi instance = RestService.For<ICsfmApi>(SERVER_URL);

        public ApiClient()
        {
        }


        public string RetrieveBearer()
        {
            return CSFMPrefs.Prefs.GetString("bearer", "");
        }

        public void ProvideBearer(string bearer)
        {
            var editor = CSFMPrefs.Editor;
            editor.PutString("bearer", bearer);
            editor.Commit();
        }

        public bool LogIn(string username, string password)
        {
            Dictionary<String, object> informations = new Dictionary<String, object>();
            informations.Add("username", username);
            informations.Add("password", password);
            var status = instance.SignIn(informations);


            var editor = CSFMPrefs.Editor;
            editor.PutString("bearer", "BEARER"); // TODO
            editor.Commit();

            return true; // TODO
        }

        public bool SignUp(string email, string username, string password)
        {
            Dictionary<String, object> informations = new Dictionary<String, object>();
            informations.Add("username", username);
            informations.Add("email", email);
            informations.Add("password", password);

            var status = instance.SignUp(informations);
            // TODO
            return true; // TODO
        }

        public async System.Threading.Tasks.Task<bool> GetUser(string username)
        {
            var user = await instance.GetUser(username, "Bearer " + this.RetrieveBearer());
            // TODO
            return true; // TODO
        }
    }
}
 