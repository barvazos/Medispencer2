using Android.App;
using System.Net;
using System.Text;  // for class Encoding
using System.IO;

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
    [Activity(Label = "Medispencer", Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        //private static readonly HttpClient client = new HttpClient();

        public void TodayMeds()
        {
            TextView todayActivityTextView = FindViewById<TextView>(Resource.Id.todayActivityTextView);
            todayActivityTextView.Text = GetTodayActivity();
        }

        public void InventoryDisplay()
        {
            
        }

        public void StatisticsDisplay()
        {
            
        }

        public void PerscriptionDisplay()
        {
            
        }
	
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            /*
            SetContentView(Resource.Layout.Main);
            
			// Get our UI controls from the loaded layout:
			EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
			Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
			Button callButton = FindViewById<Button>(Resource.Id.CallButton);

			// Disable the "Call" button
			callButton.Enabled = false;

			// Add code to translate number
			string translatedNumber = string.Empty;
            
			translateButton.Click += (object sender, EventArgs e) =>
			{
                Reminder(DateTime.Now);
			};
		   */
            SetContentView(Resource.Layout.MainPage);

            LinearLayout todayButton        = FindViewById<LinearLayout>(Resource.Id.TodayButton);
            LinearLayout inventoryButton    = FindViewById<LinearLayout>(Resource.Id.InventoryButton);
            LinearLayout statisticsButton   = FindViewById<LinearLayout>(Resource.Id.StatisticsButton);
            LinearLayout perscriptionButton = FindViewById<LinearLayout>(Resource.Id.PerscriptionButton);
            todayButton.Click           += (object sender, EventArgs e) => { TodayMeds(); };
            inventoryButton.Click       += (object sender, EventArgs e) => { InventoryDisplay(); };
            statisticsButton.Click      += (object sender, EventArgs e) => { StatisticsDisplay(); };
            perscriptionButton.Click    += (object sender, EventArgs e) => { PerscriptionDisplay(); };

            TodayMeds();

            if (!m_notificationSent)
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    WaitForAlertRequests();
                })).Start();
            }

        }

        private static bool m_notificationSent = false;

        public string GetTodayActivity()
        {
            string activity = "Today's Activity:\n\n" + 
                               "Morning: \n" +
                               "08:00:00: 1 Pill Of Adex\n\n" +
                               "Noon: \n" +
                               "13:00:00: 1 Pill Of GreanPill\n\n" +
                               "Afternoon: \n" +
                               "20:00:00: 1 Pill Of Akamol\n" +
                               "20:00:00: 1 Pill Of Adex\n\n"
                               ;
            return activity;
        }
        
        public void WaitForAlertRequests()
        {
            while(!m_notificationSent)
            {
                var request = HttpWebRequest.Create(string.Format(@"http://192.168.1.6:8080/app"));
                request.Method = "GET";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (content.Equals("True"))
                        {
                            m_notificationSent = true;
                        }
                    }
                }
            }
            //SendNotification("Take your Pills!", "Pills are waiting for you in the Medispencer");
            Reminder(DateTime.Now, "Take your Pills!", "Pills are waiting for you in the Medispencer");
         
        }

        public void SendNotification(string title = "StamTitle", string message = "StamString")
        {
            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.briefcase);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);

        }

        public void Reminder(DateTime time, string title = "StamTitle", string message = "StamString")
		{
            Intent alarmIntent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
			alarmIntent.PutExtra("message", message);
			alarmIntent.PutExtra("title", title);

			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)this.GetSystemService(Context.AlarmService);

			//TODO: For demo set after 5 seconds.
            alarmManager.Set(AlarmType.RtcWakeup, DateTime.Now.Millisecond /*+ 5 * 1000*/, pendingIntent);

		}
    }
}

