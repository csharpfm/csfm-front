using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace csfm_android
{
    [Activity(Label = "csfm_android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "MatchFM";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }
    }
}

