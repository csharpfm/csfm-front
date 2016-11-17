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
    /// <summary>
    /// First activity when you launch the app : Login or go to Sign Up activity
    /// </summary>
    [Activity(Label = Configuration.LABEL, MainLauncher = true, Theme = Configuration.LOGIN_THEME)]
    public class LoginActivity : AppCompatActivity
    {

        private Button signInButton;

        private TextView createAccount;

        private EditText username;

        private EditText password;

        private ProgressDialog progressDialog;
        private bool IsProgressDialog
        {
            set
            {
                if (value)
                {
                    progressDialog = ProgressDialog.Show(this, "", GetString(Resource.String.login_progress));
                }
                else
                {
                    progressDialog?.Cancel();
                    progressDialog = null;
                }
            }
        }

        /// <summary>
        /// On creation of the activity
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string bearer = CSFMPrefs.Bearer;

            if (CSFMApplication.IsDebug || !string.IsNullOrEmpty(bearer))
            {
                StartActivity(typeof(MainActivity));
                Finish();
            }

            SetContentView(Resource.Layout.login_activity);

            this.signInButton = FindViewById<Button>(Resource.Id.sign_in_button);
            this.createAccount = FindViewById<TextView>(Resource.Id.create_one_text);
            this.username = FindViewById<EditText>(Resource.Id.login_username_text);
            this.password = FindViewById<EditText>(Resource.Id.login_pwd_txt);

            this.InitListeners();
        }

        /// <summary>
        /// Init the listeners
        /// </summary>
        private void InitListeners()
        {
            this.createAccount.Click += delegate
            {
                StartActivity(typeof(SignupActivity));
            };

            this.signInButton.Click += delegate
            {
                this.LogIn();
            };
        }

        /// <summary>
        /// On login button click : API Request to login with the specified input info
        /// </summary>
        private void LogIn()
        {
            if (string.IsNullOrEmpty(this.username.Text))
            {
                //Empty username
                Toast.MakeText(this, Resource.String.no_username, ToastLength.Short).Show();
            }
            else if (string.IsNullOrEmpty(this.password.Text))
            {
                //Empty password
                Toast.MakeText(this, Resource.String.no_password, ToastLength.Short).Show();
            }
            else
            {
                //API Request

                IsProgressDialog = true; //Show 'loading' dialog

                Action successCallback = () =>
                {
                    StartActivity(typeof(MainActivity));
                    Finish();
                };

                Action errorCallback = () =>
                {
                    IsProgressDialog = false;
                    Toast.MakeText(this, Resource.String.error_login, ToastLength.Short).Show();
                };

                //Async
                new ApiClient().LogIn(this.username.Text, this.password.Text, successCallback, errorCallback);
            }
        }


    }
}