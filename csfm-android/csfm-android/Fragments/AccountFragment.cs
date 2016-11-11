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
using Square.Picasso;
using csfm_android.Ui.Utils;
using System.IO;
using csfm_android.Api.Interfaces;
using Java.IO;
using csfm_android.Api;
using System.Threading.Tasks;

namespace csfm_android.Fragments
{
    public class AccountFragment : Fragment
    {
        private View rootView;

        private Button signoutButton;
        private ImageView profilePicture;

        private Android.Net.Uri ProfilePictureUri
        {
            set
            {
                Picasso.With(Application.Context)
                    .Load(value)
                    .Transform(new CircleTransform())
                    .Into(profilePicture);
            }
        }

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
                    try
                    {
                        ProfilePictureUri = selectedImageUri;
                        var apiClient = new ApiClient();
                        apiClient.UploadProfilePicture("Siliem", GetBytes(selectedImageUri));
                    }
                    catch(Exception e)
                    {
                        System.Console.WriteLine(e);
                    }

                }
            }
        }

        private byte[] GetBytes(Android.Net.Uri uri)
        {
            return GetBytes(Activity.ContentResolver.OpenInputStream(uri));
        }

        private byte[] GetBytes(Stream stream)
        {
            ByteArrayOutputStream byteBuffer = new ByteArrayOutputStream();
            int bufferSize = 30000;
            byte[] buffer = new byte[bufferSize];

            int len = 0;
            while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                byteBuffer.Write(buffer, 0, len);
            }
            return byteBuffer.ToByteArray();
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
 