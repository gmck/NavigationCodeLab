using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.Navigation;

namespace com.companyname.NavigationCodeLab.Fragments
{
    public class FlowStepFragment : Fragment
    {
        public FlowStepFragment() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            HasOptionsMenu = true;
            int flowStepNumber = (int)(Arguments?.GetInt("flowStepNumber"));
            
            if (flowStepNumber == 2)
                return inflater.Inflate(Resource.Layout.flow_step_two_fragment, container, false);
            else
                return inflater.Inflate(Resource.Layout.flow_step_one_fragment, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            view.FindViewById<View>(Resource.Id.next_button).SetOnClickListener(Navigation.CreateNavigateOnClickListener(Resource.Id.next_action));
        }
    }
}