using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace com.companyname.NavigationCodeLab.Fragments
{

    public class ShoppingCartFragment : Fragment
    {
        public ShoppingCartFragment() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            HasOptionsMenu = true;
            return inflater.Inflate(Resource.Layout.shopping_fragment, container, false);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            menu.Clear();
        }
    }
}