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
using Android.Support.V7.App;
using Android.Graphics;
using csfm_android.Utils;
using csfm_android.Api;

namespace csfm_android.Activities
{
    [Activity(Label = "MatchFM", MainLauncher = true, Theme = "@style/LogTheme")]
    public class LoginActivity : AppCompatActivity
    {

        private Button signInButton;

        private TextView createAccount;

        private EditText username;

        private EditText password;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var bearer = CSFMPrefs.Prefs.GetString(CSFMApplication.BearerToken, "");

            if (CSFMApplication.IsDebug || !String.IsNullOrEmpty(bearer))
            {
                StartActivity(typeof(MainActivity));
            }

            SetContentView(Resource.Layout.login_activity);

            this.signInButton = FindViewById<Button>(Resource.Id.sign_in_button);
            this.createAccount = FindViewById<TextView>(Resource.Id.create_one_text);

            this.username = FindViewById<EditText>(Resource.Id.login_username_text);
            this.password = FindViewById<EditText>(Resource.Id.login_pwd_txt);

            this.createAccount.Click += delegate
            {
                StartActivity(typeof(SignupActivity));
            };

            this.signInButton.Click += delegate
            {
                this.LogIn();
            };
        }

        private async void LogIn()
        {
            if (String.IsNullOrEmpty(this.username.Text))
            {
                Toast.MakeText(this, Resource.String.no_username, ToastLength.Short).Show();
            }
            else if (String.IsNullOrEmpty(this.password.Text))
            {
                Toast.MakeText(this, Resource.String.no_password, ToastLength.Short).Show();
            }
            else
            {
                var apiClient = new ApiClient();
                var status = await apiClient.LogIn(this.username.Text, this.password.Text);
                
                if (status)
                {
                    StartActivity(typeof(MainActivity));
                    Finish();
                }
            }
        }


    }
}