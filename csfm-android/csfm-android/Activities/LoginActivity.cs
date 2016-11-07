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

namespace csfm_android.Activities
{
    [Activity(Label = "LoginActivity", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/LogTheme")]
    public class LoginActivity : AppCompatActivity
    {

        private Button signInButton;

        private TextView createAccount;

        private EditText email;

        private EditText password;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.login_activity);

            this.signInButton = FindViewById<Button>(Resource.Id.sign_in_button);
            this.createAccount = FindViewById<TextView>(Resource.Id.create_one_text);

            this.email = FindViewById<EditText>(Resource.Id.login_email_text);
            this.password = FindViewById<EditText>(Resource.Id.login_pwd_txt);

            this.createAccount.Click += delegate
            {
                StartActivity(typeof(SignupActivity));
            };

            this.signInButton.Click += delegate
            {
                this.logIn();
            };
        }


        private void logIn()
        {
            if (String.IsNullOrEmpty(this.email.Text))
            {
                Toast.MakeText(this, Resource.String.no_mail, ToastLength.Short).Show();
            }
            else if (String.IsNullOrEmpty(this.password.Text))
            {
                Toast.MakeText(this, Resource.String.no_password, ToastLength.Short).Show();
            } 
            else
            {
                // TODO : API CALL
                StartActivity(typeof(MainActivity));
            }
        }

        
    }
}