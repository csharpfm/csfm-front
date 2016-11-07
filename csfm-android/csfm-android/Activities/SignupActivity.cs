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

namespace csfm_android.Activities
{
    [Activity(Label = "SignupActivity", Theme = "@style/LogTheme")]
    public class SignupActivity : AppCompatActivity
    {

        private EditText email;

        private EditText password;

        private EditText confirmedPassword;

        private Button signUpButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signup_activity);

            this.email = FindViewById<EditText>(Resource.Id.signup_email);
            this.password = FindViewById<EditText>(Resource.Id.signup_pwd);
            this.confirmedPassword = FindViewById<EditText>(Resource.Id.signup_confirm_pwd);
            this.signUpButton = FindViewById<Button>(Resource.Id.sign_up_button);

            this.signUpButton.Click += delegate
            {
                this.signUp();
            };
        }

        private void signUp()
        {
            if (String.IsNullOrEmpty(this.email.Text))
            {
                Toast.MakeText(this, Resource.String.no_mail, ToastLength.Short).Show();
            }
            else if (String.IsNullOrEmpty(this.password.Text))
            {
                Toast.MakeText(this, Resource.String.no_password, ToastLength.Short).Show();
            }
            else if (String.IsNullOrEmpty(this.confirmedPassword.Text))
            {
                Toast.MakeText(this, Resource.String.no_confirmed_password, ToastLength.Short).Show();
            }
            else
            {
                // TODO API CALL
            }
        }
    }
}