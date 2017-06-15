
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

namespace TestXamarinAndroid
{
    [Activity(Label = "Medispencer", MainLauncher = true)]
    public class Login : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			// Create your application here
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Login);

            //Button loginButton = FindViewById<Button>(Resource.Id.welcomeImage);
            ImageView loginButton = FindViewById<ImageView>(Resource.Id.welcomeImage);

            loginButton.Click += (object sender, EventArgs e) =>
			{
                GoToApp();

            };

		}

        public void GoToApp()
        {
            //var intent = new Intent(this, typeof(MainActivity));
            var intent = new Intent(this, typeof(FillMedispencer));
            StartActivity(intent);
        }
    }
}
