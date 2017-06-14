using Android.App;
using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;



namespace TestXamarinAndroid
{
    [Activity(Label = "Phone Word", Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public void TodayMeds(){

        }

        public void InventoryDisplay(){
            
        }

        public void StatisticsDisplay(){
            
        }

        public void PerscriptionDisplay(){
            
        }
	
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MainPage);

            LinearLayout todayButton        = FindViewById<LinearLayout>(Resource.Id.TodayButton);
            LinearLayout inventoryButton    = FindViewById<LinearLayout>(Resource.Id.InventoryButton);
            LinearLayout statisticsButton   = FindViewById<LinearLayout>(Resource.Id.StatisticsButton);
            LinearLayout perscriptionButton = FindViewById<LinearLayout>(Resource.Id.PerscriptionButton);
            todayButton.Click           += (object sender, EventArgs e) => { TodayMeds(); };
            inventoryButton.Click       += (object sender, EventArgs e) => { InventoryDisplay(); };
            statisticsButton.Click      += (object sender, EventArgs e) => { StatisticsDisplay(); };
            perscriptionButton.Click    += (object sender, EventArgs e) => { PerscriptionDisplay(); };

		}

		public void Reminder(DateTime time, string title = "StamTitle", string message = "StamString")
		{
            Intent alarmIntent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
			alarmIntent.PutExtra("message", message);
			alarmIntent.PutExtra("title", title);

			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)this.GetSystemService(Context.AlarmService);

			//TODO: For demo set after 5 seconds.
            alarmManager.Set(AlarmType.RtcWakeup, DateTime.Now.Millisecond + 5 * 1000, pendingIntent);

		}
    }
}

