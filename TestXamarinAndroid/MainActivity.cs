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

        private void SetMainPageData(string mainPageString, int chosenLayoutId, bool showImage=false)
        {
            // update text
            TextView todayActivityTextView = FindViewById<TextView>(Resource.Id.mainPageTextView);
            todayActivityTextView.Text = mainPageString;

            // update buttons
            LinearLayout todayButton = FindViewById<LinearLayout>(Resource.Id.TodayButton);
            LinearLayout inventoryButton = FindViewById<LinearLayout>(Resource.Id.InventoryButton);
            LinearLayout statisticsButton = FindViewById<LinearLayout>(Resource.Id.StatisticsButton);
            LinearLayout perscriptionButton = FindViewById<LinearLayout>(Resource.Id.PerscriptionButton);
            todayButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#5b6e7c"));
            inventoryButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#5b6e7c"));
            statisticsButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#5b6e7c"));
            perscriptionButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#5b6e7c"));

            LinearLayout chosenButton = FindViewById<LinearLayout>(chosenLayoutId);
            chosenButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#ec008c"));

            // update image
            ImageView mainPageImageView = FindViewById<ImageView>(Resource.Id.mainPageImageView);
            if (showImage)
            {
                mainPageImageView.Visibility = ViewStates.Visible;
            }
            else
            {
                mainPageImageView.Visibility = ViewStates.Gone;
            }
            
        }

        public void TodayMeds()
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
            SetMainPageData(activity, Resource.Id.TodayButton, false); 
        }

        public void InventoryDisplay()
        {
            string status = "Inventory:\n\n" +
                            "Adex: 12 pills left\n" +
                            "GreanPill: 30 pills left\n" +
                            "Akamol: 20 pills left\n";
            SetMainPageData(status, Resource.Id.InventoryButton, false);
        }

        public void StatisticsDisplay()
        {
            SetMainPageData("", Resource.Id.StatisticsButton, true);
        }

        public void PerscriptionDisplay()
        {
            string perscription = "Perscriptions:\n\n" +
                                  "Perscription: \n" +
                                  "Pill: Adex\n" +
                                  "Duratiopn: 5 days\n" +
                                  "When: 08:00:00, 20:00:00\n" +
                                  "Dr final\n"
                               ;
            SetMainPageData(perscription, Resource.Id.PerscriptionButton, false);

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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

            if (m_startWaitForAlert)
            {
                m_startWaitForAlert = false;
                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    WaitForAlertRequests();
                })).Start();
            }

        }

        private static bool m_startWaitForAlert = true;

        public void WaitForAlertRequests()
        {
            bool notificationSent = false;
            while(!notificationSent)
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
                            notificationSent = true;
                            m_startWaitForAlert = true;
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

