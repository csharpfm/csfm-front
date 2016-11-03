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
    [Activity(Label = "LoginActivity", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class LoginActivity : AppCompatActivity
    {

        private Button signInButton;

        private TextView createAccount;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "login activity" layout resource
            SetContentView(Resource.Layout.login_activity);

            this.signInButton = FindViewById<Button>(Resource.Id.sign_in_button);
            this.createAccount = FindViewById<TextView>(Resource.Id.create_one_text);

            this.createAccount.Click += delegate
            {
                StartActivity(typeof(SignupActivity));
            };
        }

        
    }
}