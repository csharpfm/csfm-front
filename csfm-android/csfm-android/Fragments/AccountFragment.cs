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

namespace csfm_android.Fragments
{
    public class AccountFragment : Fragment, IDialogInterfaceOnClickListener
    {
        private View rootView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            this.rootView = inflater.Inflate(Resource.Layout.account_fragment, container, false);
            
            return this.rootView;
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            switch (which)
            {
                case -1:
                    //
                    break;
                case -2:
                    break;
                default:
                    break;
            }
        }


        /*  this.signInButton.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                builder.SetView(inflater.Inflate(Resource.Layout.signin_dialog, null))
                .SetPositiveButton(Resource.String.signin, this)
                .SetNegativeButton(Resource.String.cancel, this);
                // Create the AlertDialog
                AlertDialog dialog = builder.Create();
                dialog.Show();
            };

            this.signUpButton.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                builder.SetView(inflater.Inflate(Resource.Layout.signup_dialog, null))
                .SetPositiveButton(Resource.String.signup, this)
                .SetNegativeButton(Resource.String.cancel, this);
                // Create the AlertDialog
                AlertDialog dialog = builder.Create();
                dialog.Show();
            };*/
    }



}
 