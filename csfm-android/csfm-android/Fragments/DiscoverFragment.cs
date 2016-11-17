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
    /// <summary>
    /// The discover fragment : Match with people specially recommended for you
    /// </summary>
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

        /// <summary>
        /// Updates TextView that are user-dependant : Username TextView, Avatar ImageView. Also empties FavoriteSong TextView
        /// </summary>
        private User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                if (currentUser != null)
                {
                    this.AvatarUrl = currentUser.Photo;
                    this.username.Text = currentUser.Username;
                }
                else
                {
                    this.username.Text = "";
                }
                this.favoriteSong.Text = "";
            }
        }

        /// <summary>
        /// Loads image url into Avatar ImageView
        /// </summary>
        public string AvatarUrl
        {
            set
            {
                Picasso.With(this.Activity)
                    .Load(value)
                    .Into(this.avatar);
            }
        }

        /// <summary>
        /// On fragment creation
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// On fragment end
        /// </summary>
        public override void OnStop()
        {
            base.OnStop();
            ((ToolbarActivity)Activity).Toolbar.Show();
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

        /// <summary>
        /// On fragment resume
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this.recommendedUsers = new List<User>();
            InitListeners();
            GetRecommendations(); //Async API Request
        }

        /// <summary>
        /// Inits the listeners
        /// </summary>
        private void InitListeners()
        {
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
        }

        /// <summary>
        /// Inits the buttons
        /// </summary>
        private void InitButtons()
        {
            Bitmap likeBitmap = ((BitmapDrawable)this.likeButton.Drawable).Bitmap;
            this.likeButton.SetImageDrawable(new BitmapDrawable(Resources, AddGradient(likeBitmap, new Color(89, 202, 167), new Color(118, 217, 204))));

            Bitmap nextBitmap = ((BitmapDrawable)this.nextButton.Drawable).Bitmap;
            this.nextButton.SetImageDrawable(new BitmapDrawable(Resources, AddGradient(nextBitmap, new Color(228, 58, 115), new Color(238, 174, 162))));
        }

        /// <summary>
        /// Adds gradient color to the buttons
        /// </summary>
        /// <param name="originalBitmap"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
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

        /// <summary>
        /// API Request to match or dismatch a user
        /// </summary>
        /// <param name="isMatch"></param>
        private async void PutMatch(bool isMatch)
        {
            await new ApiClient().PutUserMatch(CSFMPrefs.Username, currentUser.Id, isMatch);
        }

        /// <summary>
        /// Loads the next recommended user
        /// </summary>
        private async void Next()
        {
            if (this.recommendedUsers.Any())
            {
                User user = this.recommendedUsers.First();
                this.CurrentUser = user; //Sets name text, avatar to user info ; Empties favorite song text view

                var topArtists = await new ApiClient().GetUserTopArtists(user.Username);

                if (topArtists != null && topArtists.Any())
                {
                    this.favoriteSong.Text = topArtists.First().Name;
                } 
                else
                {
                    this.favoriteSong.Text = GetString(Resource.String.no_favorite_artist);
                }
                this.recommendedUsers.Remove(user);
            }
            else
            {
                LoadMore();
            }
        }

        /// <summary>
        /// Displays the Load more button
        /// </summary>
        private void LoadMore()
        {
            this.avatar.SetImageResource(0);
            this.CurrentUser = null; //Sets user name, and favorite text to null
            this.loadMore.Visibility = ViewStates.Visible;
        }

        /// <summary>
        /// Displays the recommended users after retrieving them from an API Request
        /// </summary>
        private async void GetRecommendations()
        {
            this.recommendedUsers = await new ApiClient().GetUserRecommendations(CSFMPrefs.Username);

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
