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
using static Android.Views.View;
using Android.Provider;
using Android.Database;

namespace csfm_android.Fragments
{
    public class AccountFragment : Fragment
    {
        private View rootView;

        private Button signoutButton;
        private ImageView profilePicture;

        public object Cursor { get; private set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            this.rootView = inflater.Inflate(Resource.Layout.account_fragment, container, false);
            this.signoutButton = this.rootView.FindViewById<Button>(Resource.Id.signout);

            this.signoutButton.Click += delegate
            {
                this.SignOut();
            };

            this.profilePicture = this.rootView.FindViewById<ImageView>(Resource.Id.profilePicture);
            this.profilePicture.SetOnClickListener(new UploadPictureClickListener(this));
            return this.rootView;
        }


        private void SignOut()
        {
            CSFMPrefs.Editor.Remove(CSFMApplication.BearerToken).Commit();

            Activity.Finish();

            Intent intent = new Intent(this.Activity, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void OpenGallery(int requestCode)
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select file to upload"), requestCode);
        }

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent intent)
        {
            if (resultCode == Result.Ok)
            {
                Android.Net.Uri selectedImageUri = intent.Data;
                if (requestCode == 1)
                {
                    string selectedPath = GetPath(selectedImageUri);
                    Console.WriteLine(selectedPath);
                }
            }
        }

        public string GetPath(Android.Net.Uri uri)
        {
            string[] projection = { MediaStore.Images.Media.InterfaceConsts.Data };
            //ICursor cursor = Activity.ManagedQuery(uri, projection, null, null, null);
            ICursor cursor = Activity.ApplicationContext.ContentResolver.Query(uri, projection, null, null, null);
            int columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
            cursor.MoveToFirst();

            return cursor.GetString(columnIndex);
        }
        

        private class UploadPictureClickListener : Java.Lang.Object, IOnClickListener
        {
            private AccountFragment f;

            public UploadPictureClickListener(AccountFragment f)
            {
                this.f = f;
            }

            public void OnClick(View v)
            {
                this.f.OpenGallery(1);
            }
        }

    }



}
 