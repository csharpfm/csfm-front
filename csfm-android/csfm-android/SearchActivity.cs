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
using Android.Graphics;

namespace csfm_android
{
    [Activity(Label = Configuration.LABEL, Icon = "@drawable/icon", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class SearchActivity : ToolbarActivity, View.IOnClickListener
    {
        private ResourceButton[] btn = new ResourceButton[3];
        private Button btn_unfocus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState, Resource.Layout.activity_button_group);

            btn[0] = new ResourceButton(Resource.Id.btn0, this);
            btn[1] = new ResourceButton(Resource.Id.btn1, this);
            btn[2] = new ResourceButton(Resource.Id.btn2, this);

            for (int i = 0; i < btn.Length; i++)
            {
                btn[i].Button.SetBackgroundColor(Color.Rgb(207, 207, 207));
                btn[i].Button.SetOnClickListener(this);

            }
            
        }

        public void OnClick(View v)
        {
            int id = v.Id;
            SetFocus(btn.FirstOrDefault(b => b.ResourceId == id)?.Button);
           
        }

        private void SetFocus(Button selected)
        {
            Console.WriteLine(selected);
            if (selected == null) return;

            if (this.btn_unfocus != null)
            {
                this.btn_unfocus.SetTextColor(Color.Rgb(49, 50, 51));
                this.btn_unfocus.SetBackgroundColor(Color.Rgb(207, 207, 207));
            }

            selected.SetTextColor(Color.Rgb(255, 255, 255));
            selected.SetBackgroundColor(Color.Rgb(3, 106, 150));

            this.btn_unfocus = selected;
        }

        private class ResourceButton
        {
            public Button Button { get; set; }
            public int ResourceId { get; set; }

            public ResourceButton(int id, Activity activity)
            {
                this.ResourceId = id;
                this.Button = activity.FindViewById<Button>(this.ResourceId);
            }
        }
    }


}