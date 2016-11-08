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
    [Activity(Label = "SignupActivity", Theme = "@style/LogTheme")]
    public class SignupActivity : AppCompatActivity
    {

        private EditText username;

        private EditText email;

        private EditText password;

        private Button signUpButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signup_activity);

            this.username = FindViewById<EditText>(Resource.Id.signup_username);
            this.email = FindViewById<EditText>(Resource.Id.signup_email);
            this.password = FindViewById<EditText>(Resource.Id.signup_pwd);
            this.signUpButton = FindViewById<Button>(Resource.Id.sign_up_button);

            this.signUpButton.Click += delegate
            {
                this.signUp();
            };
        }

        private async void signUp()
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
                var apiClient = new ApiClient();
                var valid = await apiClient.SignUp(this.email.Text, this.username.Text, this.password.Text);

                if (valid)
                {
                    Finish();
                }
            }
        }
    }
}