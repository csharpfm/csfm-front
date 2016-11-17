using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace csfm_android.Ui.Utils
{
    /// <summary>
    /// Use with Picasso : Crop an image to circle shape
    /// </summary>
    public class CircleTransform : Java.Lang.Object, Square.Picasso.ITransformation
    {
        public string Key
        {
            get
            {
                return "circle";
            }
        }

        /// <summary>
        /// Used internally by Picasso
        /// </summary>
        /// <param name="p0"></param>
        /// <returns></returns>
        public Bitmap Transform(Bitmap p0)
        {
            int size = Math.Min(p0.Width, p0.Height);

            int x = (p0.Width - size) / 2;
            int y = (p0.Height - size) / 2;

            Bitmap squaredBitmap = Bitmap.CreateBitmap(p0, x, y, size, size);
            if (squaredBitmap != p0)
            {
                p0.Recycle();
            }

            Bitmap bitmap = Bitmap.CreateBitmap(size, size, p0.GetConfig());

            Canvas canvas = new Canvas(bitmap);
            Paint paint = new Paint();
            BitmapShader shader = new BitmapShader(squaredBitmap, BitmapShader.TileMode.Clamp, BitmapShader.TileMode.Clamp);
            paint.SetShader(shader);
            paint.AntiAlias = true;

            float r = size / 2f;
            canvas.DrawCircle(r, r, r, paint);

            squaredBitmap.Recycle();
            return bitmap;
        }
    }
}