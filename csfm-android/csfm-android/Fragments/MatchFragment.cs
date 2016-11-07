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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnStop()
        {
            base.OnStop();
            Activity.ActionBar.Show();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.rootView = inflater.Inflate(Resource.Layout.match_fragment, container, false);
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

            Activity.ActionBar.Hide();

            this.InitButtons();
            Picasso.With(this.Activity)
                .Load(Resource.Drawable.csfm_user)
                .Into(this.avatar);


            this.likeButton.Click += delegate
            {
                //TODO
            };

            this.nextButton.Click += delegate
            {
                //TODO
            };
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
    }

   
}
