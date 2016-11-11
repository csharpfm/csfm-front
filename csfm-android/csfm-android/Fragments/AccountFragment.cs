using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using csfm_android.Utils;
using csfm_android.Activities;
using csfm_android.Api;
using csfm_android.Api.Model;
using csfm_android.Ui.Utils;
using Square.Picasso;

namespace csfm_android.Fragments
{
    public class AccountFragment : Fragment
    {
        private View rootView;

        private Button signoutButton;

        private ImageView userAvatar;

        private TextView username;

        private EditText lastFmUsernameEditText;

        private Button lastFmButton;

        private TextView lastFmUsername;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            this.rootView = inflater.Inflate(Resource.Layout.account_fragment, container, false);
            this.signoutButton = this.rootView.FindViewById<Button>(Resource.Id.signout);
            this.userAvatar = this.rootView.FindViewById<ImageView>(Resource.Id.acc_user_avatar);
            this.username = this.rootView.FindViewById<TextView>(Resource.Id.acc_username);

            this.lastFmUsernameEditText = this.rootView.FindViewById<EditText>(Resource.Id.acc_lastfm_username_edittext);
            this.lastFmButton = this.rootView.FindViewById<Button>(Resource.Id.acc_lastfm_button);
            this.lastFmUsername = this.rootView.FindViewById<TextView>(Resource.Id.acc_lastfm_username);

            return this.rootView;
        }

        public override void OnStart()
        {
            base.OnStart();

            var username = CSFMPrefs.Prefs.GetString(CSFMApplication.Username, "");
            this.username.Text = GetString(Resource.String.connected_as).Replace("{name}", username);
            GetUser(username);

            this.signoutButton.Click += delegate
            {
                this.SignOut();
            };

            this.lastFmButton.Click += delegate
            {
                this.LinkLastFm();
            };
        }

        private async void GetUser(string username)
        {
            var user = await new ApiClient().GetUser(username);

            if (user != null)
            {
                Picasso.With(Activity)
                    .Load(user.Photo)
                    .Transform(new CircleTransform())
                    .Into(this.userAvatar);
            }
        }


        private void SignOut()
        {
            CSFMPrefs.Editor.Remove(CSFMApplication.BearerToken).Commit();
            CSFMPrefs.Editor.Remove(CSFMApplication.Username).Commit();

            Activity.Finish();

            Intent intent = new Intent(this.Activity, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void LinkLastFm()
        {
            string lastfmUsername = this.lastFmUsernameEditText.Text;

            if (!String.IsNullOrEmpty(lastfmUsername))
            {
                new ApiClient().ImportLastFm(lastfmUsername);
            }
            else
            {
                Toast.MakeText(Activity, Resource.String.no_username, ToastLength.Short).Show();
            }  
        }

    }



}
 