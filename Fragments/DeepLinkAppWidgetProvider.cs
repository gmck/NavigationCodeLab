using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.Navigation;

namespace com.companyname.NavigationCodeLab.Fragments
{
    /*
        App Widget that deep links you to the [DeepLinkFragment].
    */

    public class DeepLinkAppWidgetProvider : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            RemoteViews remoteViews = new RemoteViews(context.PackageName, Resource.Layout.deep_link_appwidget);

            var bundle = new Bundle();
            bundle.PutString("myarg", "From Widget");

            // TODO STEP 10 - construct and set a PendingIntent using DeepLinkBuilder
            var pendingIntent = new NavDeepLinkBuilder(context)
                    .SetGraph(Resource.Navigation.mobile_navigation)
                    .SetDestination(Resource.Id.deeplink_dest)
                    .SetArguments(bundle)
                    .CreatePendingIntent();
                    
            remoteViews.SetOnClickPendingIntent(Resource.Id.deep_link_button, pendingIntent);
            // TODO END STEP 10

            appWidgetManager.UpdateAppWidget(appWidgetIds, remoteViews);
        }


    }
}