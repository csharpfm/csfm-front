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

namespace csfm_android.Fragments
{
    public class MatchFragment : Fragment
    {
        private View rootView;

        private ImageView likeButton;
        private ImageView nextButton;
        private ImageView avatar;

        private TextView favoriteSong;
        private TextView username;

        private LinkedList<User> recommendedUsers;

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
            this.rootView = inflater.Inflate(Resource.Layout.match_fragment, container, false);

            ((ToolbarActivity) Activity).Toolbar.Hide();

            this.likeButton = this.rootView.FindViewById<ImageView>(Resource.Id.match_ok);
            this.nextButton = this.rootView.FindViewById<ImageView>(Resource.Id.match_cancel);
            this.avatar = this.rootView.FindViewById<ImageView>(Resource.Id.match_image);

            this.favoriteSong = this.rootView.FindViewById<TextView>(Resource.Id.match_song);
            this.username = this.rootView.FindViewById<TextView>(Resource.Id.match_username);

            return this.rootView;
        }

        public override void OnStart()
        {
            base.OnStart();

            this.InitButtons();

            this.recommendedUsers = new LinkedList<User>();

            User user1 = new User();
            user1.Username = "Hugoatease";
            user1.Image = "https://scontent-cdg2-1.xx.fbcdn.net/v/t1.0-9/14492433_730273613793013_3473639481244418470_n.jpg?oh=8a6bcee3852f9dfa67e95155fe336209&oe=5889175E";
            User user2 = new User();
            user2.Username = "Clément de Chereng";
            user2.Image = "https://scontent-cdg2-1.xx.fbcdn.net/v/t34.0-0/s261x260/14971176_10210410328633000_797219556_n.jpg?oh=92361adb492252d0c756e24ae9349a6e&oe=58229A8A";

            this.recommendedUsers.AddLast(user1);
            this.recommendedUsers.AddLast(user2);

            this.likeButton.Click += delegate
            {
                //TODO LIKE
                this.Next();
            };

            this.nextButton.Click += delegate
            {
                this.Next();
            };

            this.Next();
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

        private void Next()
        {
            if (this.recommendedUsers.Any())
            {
                User user = this.recommendedUsers.First();

                Picasso.With(this.Activity)
                  .Load(user.Image)
                  .Into(this.avatar);
            
                this.username.Text = user.Username;
                // this.favoriteSong.Text // TODO

                this.recommendedUsers.RemoveFirst();
            }
            else
            {
                // TODO LOAD MORE
            }
        }
    }


   
}
