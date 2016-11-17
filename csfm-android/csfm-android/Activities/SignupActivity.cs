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
using csfm_android.Api;

namespace csfm_android.Activities
{
    /// <summary>
    /// Activity used to make a new MatchFM account
    /// </summary>
    [Activity(Label = Configuration.LABEL, Theme = Configuration.LOGIN_THEME)]
    public class SignupActivity : AppCompatActivity
    {

        private EditText username;

        private EditText email;

        private EditText password;

        private Button signUpButton;

        /// <summary>
        /// On creation of the activity
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signup_activity);

            this.username = FindViewById<EditText>(Resource.Id.signup_username);
            this.email = FindViewById<EditText>(Resource.Id.signup_email);
            this.password = FindViewById<EditText>(Resource.Id.signup_pwd);
            this.signUpButton = FindViewById<Button>(Resource.Id.sign_up_button);

            InitListeners();
        }

        /// <summary>
        /// Init the listeners
        /// </summary>
        private void InitListeners()
        {
            this.signUpButton.Click += delegate
            {
                this.SignUp();
            };
        }

        /// <summary>
        /// Sends the API Request to sign up if form inputs are correct
        /// </summary>
        private void SignUp()
        {
            if (String.IsNullOrEmpty(this.username.Text))
            {
                Toast.MakeText(this, Resource.String.no_username, ToastLength.Short).Show();
            }
            else if (String.IsNullOrEmpty(this.email.Text))
            {
                Toast.MakeText(this, Resource.String.no_mail, ToastLength.Short).Show();
            }
            else if (String.IsNullOrEmpty(this.password.Text))
            {
                Toast.MakeText(this, Resource.String.no_password, ToastLength.Short).Show();
            }
            else
            {
                Action successCallback = () => Finish();
                Action errorCallback = () => Toast.MakeText(this, Resource.String.error_sign_up, ToastLength.Short).Show();

                new ApiClient().SignUp(this.email.Text, this.username.Text, this.password.Text, successCallback, errorCallback);
            }
        }
    }
}