using Android.App;
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
using System;

namespace com.companyname.NavigationCodeLab
{
    /*
        * An activity demonstrating use of a NavHostFragment with a navigation drawer and bottom.
    */
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    // Step 11 from the code lab. 
    // It is not possible to add the following line <nav-graph android:value="@navigation/mobile_navigation" /> 
    // to the Xamarin.Android AndroidManifest.xml. See https://docs.microsoft.com/en-us/xamarin/android/platform/android-manifest
    // Below we can achieve the same thing. See the AndroidManifest that is generated in NavigationCodeLab\obj\Debug to see how the IntentFilter below is transformed.
    // Then compare to the AndroidManifest in the project properties. If you are familiar with Xamarin.Auth you will see a very similar technique in the CustomUrlSchemeInterceptorActivity

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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.navigation_activity);

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


        // Original codelab code - see an alternative below
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //return base.OnOptionsItemSelected(item);

            // TODO STEP 9.2 - Have Navigation UI Handle the item selection - make sure to delete or comment
            // the original return statement above

            // Have the NavigationUI look for an action or destination matching the menu
            // item id and navigate there if found.
            // Otherwise, bubble up to the parent.
            // return item.onNavDestinationSelected(findNavController(R.id.my_nav_host_fragment)) || super.onOptionsItemSelected(item)

            // This Kotlin code is probably the most complex code to translate from Kotlin to C#. The Kotlin code really doesn't give you much of a clue!!. At least the comment mentions Navigation UI.
            // Tip: just check the class definations of NavigationUI and Navigation - right click on either and Go To Definition 
            // Note: OnNavDestinationSelected will immediately call OnDesinationChanged.

            // A long hand way of writing the same thing.
            // if Navigation.FindNavController(etc..) succeeds ie finds a matching item.id in the nav_host_fragment then pass the item and navController to NavigationUI.OnDestinationSelected 
            // else just return base.OnOptionsItemSelected.

            //NavController navController = Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment);
            //if (navController != null)
            //    return NavigationUI.OnNavDestinationSelected(item, navController);
            //else
            //    return base.OnOptionsItemSelected(item);

            return NavigationUI.OnNavDestinationSelected(item, Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment)) || base.OnOptionsItemSelected(item);

            // TODO END STEP 9.2
        }

        // Alternative, showing how we could display a dialog from the overflow menu without having the dialog in the nav_graph
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //        // Kotlin
        //        // NavController navController = Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment);

        //        if (item.ItemId == Resource.Id.action_google_sign_in)
        //        {
        //            // InternetConnectionAvailable is just a dummy test function - returns true of false. Toggle the return value to  get either the dialog or show the fragment
        //            if (!InternetConnectionAvailable())
        //                ShowInternetRequiredDialog("GoogleSignIn");
        //            else
        //                Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment).Navigate(Resource.Id.action_google_sign_in);

        //            return true;
        //        }
        //        else
        //            return NavigationUI.OnNavDestinationSelected(item, Navigation.FindNavController(this, Resource.Id.my_nav_host_fragment)) || base.OnOptionsItemSelected(item);
        //}


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

        Toast.MakeText(this, "Navigated to " + destination, ToastLength.Short).Show();
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
        // dummy function as an example of what may be need before showing a particular fragment such as the Google sign In fragment
        bool available = false;

        return available;
    }
    #endregion

    #region ShowInternetRequiredDialog
    internal void ShowInternetRequiredDialog(string choice)
    {
        // Has DismissDialog
        InternetRequiredDialogFragment internetRequiredDialogFragment = InternetRequiredDialogFragment.NewInstance(GetString(Resource.String.internetConnectionRequired), choice);
        internetRequiredDialogFragment.Show(SupportFragmentManager, "InternetRequiredDialogFragment");
    }
    #endregion

    #region DismissInternetRequiredDialog
    internal void DismissInternetRequiredDialog()
    {
        InternetRequiredDialogFragment internetRequiredDialogFragment = (InternetRequiredDialogFragment)SupportFragmentManager.FindFragmentByTag("InternetRequiredDialogFragment");
        if (internetRequiredDialogFragment != null)
        {
            if (internetRequiredDialogFragment.Dialog.IsShowing)
                internetRequiredDialogFragment.Dialog.Dismiss();
        }
    }
    #endregion
}
}