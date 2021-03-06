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
    /// <summary>
    /// Settings fragment : View your profile, modify your settings and sign out
    /// </summary>
    public class SettingsFragment : Fragment
    {
        private View rootView;

        private TextView signOut;

        private ImageView userAvatar;

        private TextView username;

        private ImageView linkLastFmAccount;

        private Switch scrobblerSwitch;

        /// <summary>
        /// On fragment creation
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// On fragment view creation
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            this.rootView = inflater.Inflate(Resource.Layout.settings_fragment, container, false);
            this.signOut = this.rootView.FindViewById<TextView>(Resource.Id.acc_logout);
            this.userAvatar = this.rootView.FindViewById<ImageView>(Resource.Id.acc_user_avatar);
            this.username = this.rootView.FindViewById<TextView>(Resource.Id.acc_username);
            this.linkLastFmAccount = this.rootView.FindViewById<ImageView>(Resource.Id.acc_link_action);
            this.scrobblerSwitch = this.rootView.FindViewById<Switch>(Resource.Id.acc_switch_scrobble);

            this.scrobblerSwitch.Checked = CSFMPrefs.Prefs.GetBoolean(CSFMApplication.IsScrobbling, true);
            return this.rootView;
        }

        /// <summary>
        /// On fragment start
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            var username = CSFMPrefs.Username;
            this.username.Text = username;
            GetUser(username);

            this.signOut.Click += delegate
            {
                this.SignOut();
            };

            this.linkLastFmAccount.Click += delegate
            {
                Dialog dialog = new Dialog(this.Activity);

                dialog.SetTitle(GetString(Resource.String.link_an_account));
                dialog.SetContentView(Resource.Layout.lastfm_dialog);

                Button button = dialog.FindViewById<Button>(Resource.Id.dialog_link_button);
                button.Click += delegate
                {
                    EditText edit = dialog.FindViewById<EditText>(Resource.Id.dialog_edittext);
                    string usernameText = edit.Text;

                    if (!String.IsNullOrEmpty(usernameText))
                    {
                        this.LinkLastFm(usernameText);
                    }

                    dialog.Dismiss();
                };

                dialog.Show();
                DisplayMetrics metrics = Resources.DisplayMetrics;
                int width = metrics.WidthPixels;
                int height = metrics.HeightPixels;
                dialog.Window.SetLayout((6 * width) / 7, -2 /*wrap_content*/);
            };

            this.scrobblerSwitch.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) {
                CSFMPrefs.IsScrobbling = e.IsChecked;
            };
        }

        /// <summary>
        /// API Request to get and display the user info
        /// </summary>
        /// <param name="username"></param>
        private async void GetUser(string username)
        {
            var user = await new ApiClient().GetUser(username);

            if (user != null && !String.IsNullOrEmpty(user.Photo))
            {
                Picasso.With(Activity)
                    .Load(user.Photo)
                    .Transform(new CircleTransform())
                    .Placeholder(Resource.Drawable.csfm_user)
                    .Into(this.userAvatar);
            }
            else
            {
                Picasso.With(Activity)
                    .Load(Resource.Drawable.csfm_user)
                    .Transform(new CircleTransform())
                    .Into(this.userAvatar);
            }
        }

        /// <summary>
        /// Sign out the user
        /// </summary>
        private void SignOut()
        {
            var editor = CSFMPrefs.Editor;

            editor.Remove(CSFMApplication.BearerToken);
            editor.Remove(CSFMApplication.Username);
            editor.Remove(CSFMApplication.IsScrobbling);
            editor.Commit();

            Activity.Finish();

            Intent intent = new Intent(this.Activity, typeof(LoginActivity));
            StartActivity(intent);
        }

        /// <summary>
        /// Import Last fm account
        /// </summary>
        /// <param name="lastfmUsername"></param>
        private void LinkLastFm(string lastfmUsername)
        {
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
 