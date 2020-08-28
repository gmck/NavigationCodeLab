using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;
using com.companyname.NavigationCodeLab.Dialogs;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using System;

namespace com.companyname.NavigationCodeLab
{
    /*
        * An activity demonstrating use of a NavHostFragment with a navigation drawer and bottom.
    */
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    // Step 11 from the code lab. It is not possible to add the following line
    // <nav-graph android:value="@navigation/mobile_navigation" /> 
    // to the Xamarin.Android AndroidManifest.xml
    // Below we can achieve the same thing. See the AndroidManifest that is generated in NavigationCodeLab\obj\Debug to see how the IntentFilter below is transformed
    
    //[IntentFilter
    //    (
    //        actions: new[] { Intent.ActionView },
    //        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    //        DataSchemes = new[] { "http", "https" },
    //        DataHost = "www.example.com",
    //        DataPathPrefix = "/"
    //    )
    //]

    public class MainActivity : AppCompatActivity, NavController.IOnDestinationChangedListener
    {
        private AppBarConfiguration appBarConfiguration;
        private ConnectivityMonitor connectivityMonitor;
        private readonly bool connectivityInfo = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.navigation_activity);

            // This must come early - failed with a null reference exception on connectivityMonitor in ConnectionAvailable() when at the end of OnCreate.
            // RegisterDefaultNetworkCallback available API 24+ Android 7+
            connectivityMonitor = new ConnectivityMonitor(ApplicationContext);
            connectivityMonitor.RegisterDefaultNetworkCallback();

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            NavHostFragment host = SupportFragmentManager.FindFragmentById(Resource.Id.my_nav_host_fragment) as NavHostFragment;
            NavController navController = host.NavController;

            //appBarConfiguration = new AppBarConfiguration.Builder(navController.Graph).Build();

            // TODO STEP 9.5 - Create an AppBarConfiguration with the correct top-level destinations
            // You should also remove the old appBarConfiguration setup above
            // val drawerLayout : DrawerLayout? = findViewById(R.id.drawer_layout)
            // appBarConfiguration = AppBarConfiguration(setOf(R.id.home_dest, R.id.deeplink_dest),drawerLayout)

            // These are the fragments that you don't wont the back button of the toolbar to display on e.g. topLevel
            int[] topLevelDestinationIds = new int[] { Resource.Id.home_dest, Resource.Id.deeplink_dest };
            appBarConfiguration = new AppBarConfiguration.Builder(topLevelDestinationIds).SetDrawerLayout(FindViewById<DrawerLayout>(Resource.Id.drawer_layout)).Build();
            // TODO END STEP 9.5

            SetupActionBar(navController, appBarConfiguration);
            SetupNavigationMenu(navController);
            SetupBottomNavMenu(navController);

            navController.AddOnDestinationChangedListener(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            InternetConnectionAvailable();
        }
        private void SetupBottomNavMenu(NavController navController)
        {
            // TODO STEP 9.3 - Use NavigationUI to set up Bottom Nav
            // val bottomNav = findViewById<BottomNavigationView>(R.id.bottom_nav_view)
            // bottomNav?.setupWithNavController(navController)
            
            var bottomNav = FindViewById<BottomNavigationView>(Resource.Id.bottom_nav_view);
            if (bottomNav != null)
                NavigationUI.SetupWithNavController(bottomNav, navController);
            // TODO END STEP 9.3
        }

        private void SetupNavigationMenu(NavController navController)
        {
            // TODO STEP 9.4 - Use NavigationUI to set up a Navigation View
            // In split screen mode, you can drag this view out from the left
            // This does NOT modify the actionbar
            // val sideNavView = findViewById<NavigationView>(R.id.nav_view)
            // sideNavView?.setupWithNavController(navController)

            var sideNavView = FindViewById<NavigationView>(Resource.Id.nav_view);
            if (sideNavView != null)
                NavigationUI.SetupWithNavController(sideNavView, navController);
            // TODO END STEP 9.4
        }

        private void SetupActionBar(NavController navController, AppBarConfiguration appBarConfiguration)
        {
            // TODO STEP 9.6 - Have NavigationUI handle what your ActionBar displays
            // This allows NavigationUI to decide what label to show in the action bar
            // By using appBarConfig, it will also determine whether to
            // show the up arrow or drawer menu icon
            NavigationUI.SetupActionBarWithNavController(this, navController, appBarConfiguration);
            // TODO END STEP 9.6
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            bool retValue = base.OnCreateOptionsMenu(menu);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            // The NavigationView already has these same navigation items, so we only add
            // navigation items to the menu here if there isn't a NavigationView
            if (navigationView == null)
            {
                MenuInflater.Inflate(Resource.Menu.overflow_menu, menu);
                return true;
            }
            return retValue;
        }


        // Orignal codelab code - see an alternative below
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    //return base.OnOptionsItemSelected(item);

        //    // TODO STEP 9.2 - Have Navigation UI Handle the item selection - make sure to delete or comment
        //    // the original return statement above

        //    // Have the NavigationUI look for an action or destination matching the menu
        //    // item id and navigate there if found.
        //    // Otherwise, bubble up to the parent.
        //    // return item.onNavDestinationSelected(findNavController(R.id.my_nav_host_fragment)) || super.onOptionsItemSelected(item)

        //    // This Kotlin is probably the most complex code to translate from Kotlin to C#. The Kotlin code really doesn't give you much of a clue!!. At least the comment mentions Navigation UI.
        //    // Tip: just check the class definations of NavigationUI and Navigation - right click on either and Go To Definition 
        //    // Note: OnNavDestinationSelected will immediately call OnDesinationChanged.

        //    // A long hand way of writing the same thing.
        //    // if Navigation.FindNavController(etc..) succeeds ie finds a matching item.id in the nav_host_fragment then pass the item and navController to NavigationUI.OnDestinationSelected 
        //    // else just return base.OnOptionsItemSelected.

        //    //NavController navController = Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment);
        //    //if (navController != null)
        //    //    return NavigationUI.OnNavDestinationSelected(item, navController);
        //    //else
        //    //    return base.OnOptionsItemSelected(item);

        //    return NavigationUI.OnNavDestinationSelected(item, Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment)) || base.OnOptionsItemSelected(item);

        //    // TODO END STEP 9.2
        //}

        // Alternative, showing how we could display a dialog from the overflow menu without having the dialog in the nav_graph
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Kotlin
            // NavController navController = Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment);

            if (item.ItemId == Resource.Id.action_privacy_policy)
            {
                ShowPrivacyPolicyDialog(GetString(Resource.String.privacy_policy_title), GetString(Resource.String.privacy_policy_explanation));
                return true;
            }
            else
                return NavigationUI.OnNavDestinationSelected(item, Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment)) || base.OnOptionsItemSelected(item);
        }


        public void OnDestinationChanged(NavController p0, NavDestination p1, Bundle p2)
        {
            // Wish Xamarin would fix these parameter name problems p0,p1,p2

            // Note:- 
            // OnDestinationChanged is executed even before OnCreate of the destination fragment and the OnStart of the MainActivity. 
            // The destination fragment's ctor has already been called before OnDestinationChanged.

            int navDestinationId = p1.Id;
            string destination;

            try { destination = Resources.GetResourceName(navDestinationId); }
            catch (Android.Content.Res.Resources.NotFoundException) { destination = Convert.ToString(navDestinationId); }

            //Toast.MakeText(this, "Navigated to " + destination, ToastLength.Short).Show();
        }

        // TODO STEP 9.7 - Have NavigationUI handle up behavior in the ActionBar
        public override bool OnSupportNavigateUp()
        {
            // Kotlin
            // Allows NavigationUI to support proper up navigation or the drawer layout drawer menu, depending on the situation
            //    override fun onSupportNavigateUp(): Boolean {
            //        return findNavController(R.id.my_nav_host_fragment).navigateUp(appBarConfiguration)
            //      }

            return NavigationUI.NavigateUp(Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment), appBarConfiguration);
        }
        // TODO END STEP 9.7

        

        #region InternetConnectionAvailable
        internal bool InternetConnectionAvailable()
        {
            bool available = false;

            if (connectivityMonitor.NetworkStatus == NetworkStatus.Connected)
            {
                available = true;
                ShowSnackbar("Internet Connected");
            }
            else if (connectivityMonitor.NetworkStatus == NetworkStatus.WifiConnected)
            {
                available = true;
                ShowSnackbar("Internet Connected - Wi-Fi.");
            }
            else if (connectivityMonitor.NetworkStatus == NetworkStatus.CellularConnected)
            {
                available = true;
                ShowSnackbar("Internet Connected - Cellular.");
            }
            else if (connectivityMonitor.NetworkStatus == NetworkStatus.WifiScantoolConnected)
                ShowSnackbar("No Internet - Connected to Wi-Fi scan tool.");
            else if (connectivityMonitor.NetworkStatus == NetworkStatus.Disconnected)
                ShowSnackbar("Internet not available.");

            return available;
        }
        #endregion

        #region ShowSnackbar
        private void ShowSnackbar(string message)
        {
            View snackbarView = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

            if (snackbarView == null)
                snackbarView = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            Snackbar snackbar = Snackbar.Make(snackbarView, message, Snackbar.LengthLong);
            snackbar.SetDuration(1250); // Make the duration shorter than a normal snackbar
            View view = snackbar.View;
            view.SetBackgroundResource(Resource.Color.colorPrimary);
            TextView tv = view.FindViewById<TextView>(Resource.Id.snackbar_text);
            if (tv != null)
            {
                tv.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
                tv.TextAlignment = TextAlignment.Center;
            }

            if (connectivityInfo)
            {
                //  Only show the Connectivity snackbar when on the Connection fragment

                // This works, but why so bloody complicated. 
                // Why do we need this extra crap, if we aren't going near the ConnectionFragment which is the StartDestination fragment
                //if (navController.Graph.StartDestination == navController.CurrentDestination.Id)
                //{
                //    // We don't want the snackbar shown on the ReadVehiclePidsFragment when exiting from any of the DashboardFragments. Why though is the Graph.StartDestination == navController.CurrentDestination.Id in the first place
                //    // when exiting either of the two pager fragments.
                //    AndroidX.Fragment.App.Fragment currentFragment = SupportFragmentManager.FindFragmentById(Resource.Id.nav_host).ChildFragmentManager.PrimaryNavigationFragment;
                //    if (!(currentFragment is DashboardPagerFragment) && !(currentFragment is FuelDashboardPagerFragment))
                //        snackbar.Show();
                //}

                snackbar.Show();
            }
        }
        #endregion

        internal void ShowPrivacyPolicyDialog(string title, string explanation)
        {
            // No DismissDialog
            BasicDialogFragment privacyPolicyDialog = BasicDialogFragment.NewInstance(title, explanation);
            privacyPolicyDialog.Show(SupportFragmentManager, "PrivacyPolicyDialogFragment");
        }
    }
}