using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace com.companyname.NavigationCodeLab.Fragments
{
    public class SettingsFragment : Fragment
    {
        public SettingsFragment() { }

        // The Android app Navigation Codelab shows an overflow menu (three dot menu) on this fragment when it portrait mode, but not in Landscape when selected via the NavigationView
        // If you click the overflow menu when the Settings fragment is showing, then you get Setting again.
        // I believe that is just a mistake in the Android app, so I've modified this fragment to not show an overflow menu.
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            HasOptionsMenu = true;
            return inflater.Inflate(Resource.Layout.settings_fragment, container, false);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            menu.Clear();
        }
    }
}