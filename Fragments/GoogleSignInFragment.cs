using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace com.companyname.NavigationCodeLab.Fragments
{
    public class GoogleSignInFragment :  Fragment
    {
        public GoogleSignInFragment() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            HasOptionsMenu = true;
            return inflater.Inflate(Resource.Layout.google_sign_in_fragment, container, false);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            menu.Clear();
        }
    }
}