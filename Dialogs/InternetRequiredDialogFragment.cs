using Android;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.Dialog;

namespace com.companyname.NavigationCodeLab.Dialogs
{
    internal class InternetRequiredDialogFragment : AppCompatDialogFragment
    {
        
        public InternetRequiredDialogFragment() { }
        
        internal static InternetRequiredDialogFragment NewInstance(string title, string choice)
        {
            Bundle arguments = new Bundle();
            arguments.PutString("Title", title);
            arguments.PutString("MenuChoice", choice);


            InternetRequiredDialogFragment fragment = new InternetRequiredDialogFragment
            {
                RetainInstance = true,
                Cancelable = false,
                Arguments = arguments
            };
            return fragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            LayoutInflater inflater = LayoutInflater.From(Activity);

            View view = inflater.Inflate(Resource.Layout.generic_dialog, null);
            TextView textViewInternetRequired = view.FindViewById<TextView>(Resource.Id.textView_explanation);
            int explanationText = 0;

            string menuChoice = Arguments.GetString("MenuChoice");
            if (menuChoice == "GoogleSignIn")
                explanationText = Resource.String.internetRequiredGoogleSignIn;
            else if (menuChoice == "Evaluation")
                explanationText = Resource.String.internetRequiredExplanationEvaluation;
            
            

            textViewInternetRequired.SetText(explanationText);

            MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
            builder.SetTitle(Arguments.GetString("Title"));
            builder.SetView(view);

            builder.SetPositiveButton(Android.Resource.String.Ok, (sender, e) => { ((MainActivity)Activity).DismissInternetRequiredDialog(); });
            return builder.Create();
        }

        public override void OnDestroyView()
        {
            // IMPORTANT requires this for a rotation. Without it the dialog disappears on rotation.
            if (Dialog != null && RetainInstance)
                Dialog.SetDismissMessage(null);  // Requires this if using RetainInstance - check when new version of v4 available
            base.OnDestroyView();
        }
    }
}