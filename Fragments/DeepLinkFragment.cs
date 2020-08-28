using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Navigation;

namespace com.companyname.NavigationCodeLab.Fragments
{
    public class DeepLinkFragment : AndroidX.Fragment.App.Fragment //, View.IOnClickListener
    {
        public DeepLinkFragment() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.deeplink_fragment, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var textView = view.FindViewById<TextView>(Resource.Id.text);
            textView.Text = Arguments?.GetString("myarg");

            //Button notificationButton = view.FindViewById<Button>(Resource.Id.send_notification_button);
            //notificationButton.SetOnClickListener(this); 

            // Alternate method to above SetOnClickListener - to use comment out the View.IOnClickListner in the class declaration and the two lines above
            view.FindViewById<Button>(Resource.Id.send_notification_button).Click += (sender, e) =>
            {
                EditText editTextArgs = view.FindViewById<EditText>(Resource.Id.args_edit_text);
                Bundle bundle = new Bundle();
                bundle.PutString("myarg", editTextArgs.Text.ToString());

                var deeplink = Navigation.FindNavController((View)sender)
                                         .CreateDeepLink()
                                         .SetDestination(Resource.Id.deeplink_dest)
                                         .SetArguments(bundle)
                                         .CreatePendingIntent();

                var notificationManager = Context?.GetSystemService(Context.NotificationService) as NotificationManager;

                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    notificationManager.CreateNotificationChannel(new NotificationChannel("deeplink", "Deep Links", NotificationImportance.High));
                    
                    Notification notification = new NotificationCompat.Builder(Context, "deeplink")
                        .SetContentTitle("Navigation")
                        .SetContentText("Deep link to Android")
                        .SetSmallIcon(Resource.Drawable.ic_android)
                        .SetContentIntent(deeplink)
                        .SetAutoCancel(true)
                        .Build();

                    notificationManager.Notify(0, notification);
                }
            };
        }

        // Alternate SetOnClickListerner method - to use uncomment this method and comment out the Button.Click above and then uncomment the two lines above that and include the View.IOnClickLister in the class declaration
        public void OnClick(View v)
        {
            EditText editTextArgs = ((View)v.Parent).FindViewById<EditText>(Resource.Id.args_edit_text);
            Bundle bundle = new Bundle();
            bundle.PutString("myarg", editTextArgs.Text.ToString());

            var deeplink = Navigation.FindNavController(v)
                                     .CreateDeepLink()
                                     .SetDestination(Resource.Id.deeplink_dest)
                                     .SetArguments(bundle)
                                     .CreatePendingIntent();

            var notificationManager = Context?.GetSystemService(Context.NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationManager.CreateNotificationChannel(new NotificationChannel("deeplink", "Deep Links", NotificationImportance.High));
                
                Notification notification = new NotificationCompat.Builder(Context, "deeplink")
                    .SetContentTitle("Navigation")
                    .SetContentText("Deep link to Android")
                    .SetSmallIcon(Resource.Drawable.ic_android)
                    .SetContentIntent(deeplink)
                    .SetAutoCancel(true)
                    .Build();

                notificationManager.Notify(0, notification);
            }
        }
    }
}