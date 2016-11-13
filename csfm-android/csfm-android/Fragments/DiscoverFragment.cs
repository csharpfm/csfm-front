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
using Android.Graphics;
using Android.Graphics.Drawables;
using Square.Picasso;
using csfm_android.Api.Model;
using csfm_android.Activities;
using csfm_android.Utils.MaterialDesignSearchView;
using csfm_android.Api;
using csfm_android.Utils;

namespace csfm_android.Fragments
{
    public class DiscoverFragment : Fragment
    {
        private View rootView;

        private ImageView likeButton;
        private ImageView nextButton;
        private ImageView avatar;

        private TextView favoriteSong;
        private TextView username;

        private Button loadMore;

        private List<User> recommendedUsers;

        private User currentUser;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnStop()
        {
            base.OnStop();
            ((ToolbarActivity)Activity).Toolbar.Show();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.rootView = inflater.Inflate(Resource.Layout.discover_fragment, container, false);

            ((ToolbarActivity)Activity).Toolbar.Hide();

            this.likeButton = this.rootView.FindViewById<ImageView>(Resource.Id.match_ok);
            this.nextButton = this.rootView.FindViewById<ImageView>(Resource.Id.match_cancel);
            this.avatar = this.rootView.FindViewById<ImageView>(Resource.Id.match_image);

            this.favoriteSong = this.rootView.FindViewById<TextView>(Resource.Id.match_song);
            this.username = this.rootView.FindViewById<TextView>(Resource.Id.match_username);

            this.loadMore = this.rootView.FindViewById<Button>(Resource.Id.load_more);

            return this.rootView;
        }

        public override void OnStart()
        {
            base.OnStart();

            this.recommendedUsers = new List<User>();

            this.likeButton.Click += delegate
            {
                if (this.currentUser != null)
                {
                    PutMatch(true);
                    this.Next();
                } 
            };

            this.nextButton.Click += delegate
            {
                if (this.currentUser != null)
                {
                    PutMatch(false);
                    this.Next();
                }
            };

            this.loadMore.Click += delegate
            {
                GetRecommendations();
            };

            GetRecommendations();
        }

        private void InitButtons()
        {
            Bitmap likeBitmap = ((BitmapDrawable)this.likeButton.Drawable).Bitmap;
            this.likeButton.SetImageDrawable(new BitmapDrawable(Resources, AddGradient(likeBitmap, new Color(89, 202, 167), new Color(118, 217, 204))));

            Bitmap nextBitmap = ((BitmapDrawable)this.nextButton.Drawable).Bitmap;
            this.nextButton.SetImageDrawable(new BitmapDrawable(Resources, AddGradient(nextBitmap, new Color(228, 58, 115), new Color(238, 174, 162))));
        }

        private Bitmap AddGradient(Bitmap originalBitmap, Color start, Color end)
        {
            int width = originalBitmap.Width;
            int height = originalBitmap.Height;
            Bitmap updatedBitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(updatedBitmap);

            canvas.DrawBitmap(originalBitmap, 0, 0, null);

            Paint paint = new Paint();
            LinearGradient shader = new LinearGradient(0, 0, 0, height, start, end, Shader.TileMode.Clamp);
            paint.SetShader(shader);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawRect(0, 0, width, height, paint);

            return updatedBitmap;
        }

        private async void PutMatch(bool isMatch)
        {
            await new ApiClient().PutUserMatch(CSFMPrefs.Prefs.GetString(CSFMApplication.Username, ""), currentUser.Id, isMatch);
        }

        private async void Next()
        {
            if (this.recommendedUsers.Any())
            {
                User user = this.recommendedUsers.First();

                this.currentUser = user;

                Picasso.With(this.Activity)
                    .Load(user.Photo)
                    .Into(this.avatar);

                this.username.Text = user.Username;

                var topArtists = await new ApiClient().GetUserTopArtists(user.Username);

                if (topArtists != null && topArtists.Any())
                {
                    this.favoriteSong.Text = topArtists.First().Name;
                }

                this.recommendedUsers.Remove(user);
            }
            else
            {
                LoadMore();
            }
        }

        private void LoadMore()
        {
            this.avatar.SetImageResource(0);
            this.username.Text = "";
            this.favoriteSong.Text = "";

            this.currentUser = null;

            this.loadMore.Visibility = ViewStates.Visible;
        }

        private async void GetRecommendations()
        {
            this.recommendedUsers = await new ApiClient().GetUserRecommendations(CSFMPrefs.Prefs.GetString(CSFMApplication.Username, ""));

            if (this.recommendedUsers != null && this.recommendedUsers.Any())
            {
                this.loadMore.Visibility = ViewStates.Gone;
                this.Next();
            }
            else
            {
                LoadMore();
            }

            this.InitButtons();
        }
    }
   
}
