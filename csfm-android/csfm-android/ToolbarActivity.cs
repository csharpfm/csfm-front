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

namespace csfm_android
{
    public class ToolbarActivity : AppCompatActivity
    {

        public Android.Support.V7.Widget.Toolbar Toolbar { get; private set; }

        protected void OnCreate(Bundle savedInstanceState, int layout)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                SetContentView(layout);

                this.Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(this.Toolbar);
                this.Toolbar.Title = Configuration.LABEL;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            throw new Exception("Use OnCreate(Bundle, int) to specifiy a resource Id");
        }
    }
}