using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.Navigation;

namespace com.companyname.NavigationCodeLab.Fragments
{
    public class HomeFragment : Fragment //, View.IOnClickListener
    {
        public HomeFragment() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            
            HasOptionsMenu = true;
            return inflater.Inflate(Resource.Layout.home_fragment, container, false);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.main_menu, menu);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // TODO STEP 5 Use a standard SetOnClickListener - see OnClick method below
            //var button = view.FindViewById<Button>(Resource.Id.navigate_destination_button);
            //button.SetOnClickListener(this);

            // A couple of variations or alternates without directly declaring the button
            //view.FindViewById<Button>(Resource.Id.navigate_destination_button).Click += (sender, e) =>
            //{
            //    Navigation.FindNavController((View)sender).Navigate(Resource.Id.flow_step_one_dest, null);
            //};

            // or another alternative is to use Navigation's CreateNavigationOnClickListener
            //var button = view.FindViewById<Button>(Resource.Id.navigate_destination_button);
            //button.SetOnClickListener(Navigation.CreateNavigateOnClickListener(Resource.Id.flow_step_one_dest, null));

            // and another without directly declaring the Button - Xamarin.Android and C# provide very flexible syntax to do the same thing. Whatever floats your boat as long as it works!!
            //view.FindViewById<Button>(Resource.Id.navigate_destination_button).SetOnClickListener(Navigation.CreateNavigateOnClickListener(Resource.Id.flow_step_one_dest, null));
            // TODO END OF STEP 5

            //TODO STEP 6 - Set NavOptions - Change from the default transition
            NavOptions navOptions = new NavOptions.Builder()
                .SetEnterAnim(Resource.Animation.slide_in_right) 
                .SetExitAnim(Resource.Animation.slide_out_left) 
                .SetPopEnterAnim(Resource.Animation.slide_in_left)
                .SetPopExitAnim(Resource.Animation.slide_out_right)
                .Build();
            
            // Now from what we saw in Step 5 - make sure all of step 5 is now commented as per the Code Lab instructions
           
            // The navigate_destination_button - note not passing a bundle, but we are passing the navOptions
            view.FindViewById<Button>(Resource.Id.navigate_destination_button).Click += (sender, e) =>
            {
                Navigation.FindNavController((View)sender).Navigate(Resource.Id.flow_step_one_dest, null, navOptions);
            };
            // TODO END STEP 6


            // TODO Set 7.2 - Update the OnClickListener to navigate using an action
            // Don't forget the Step 7.1 (in the nav_graph) before you do this step - Step 7.2
            // Note: It would be nice if we also had a Navigation.CreateNavigateOnClickListener(int resId, Bundle args, NavOptions navOptions) for SetOnClickListener here.
            // we have one taking a resid, bundle, but not resid, bundle and navOptions. Could eliminate the NavOptions in the graph.

            // Now the navigate_action_button - passing the action next_action to a CreateNavigationOnClickListener, the NavOptions in this case are coming from the nav_graph
            view.FindViewById<Button>(Resource.Id.navigate_action_button).SetOnClickListener(Navigation.CreateNavigateOnClickListener(Resource.Id.next_action, null));
            // TODO END OF STEP 7.2
        }

        // Could use Standard View.IOnClickListener - uncomment View.IOnClickListener in the declaration of this class to use it.
        public void OnClick(View v)
        {
            Navigation.FindNavController(v).Navigate(Resource.Id.flow_step_one_dest, null);
        }
    }
}