using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.Dialog;

namespace com.companyname.NavigationCodeLab.Dialogs
{
    internal class BasicDialogFragment : AppCompatDialogFragment
    {
        public BasicDialogFragment() { }

        internal static BasicDialogFragment NewInstance(string title, string message)
        {
            Bundle arguments = new Bundle();
            arguments.PutString("Title", title);
            arguments.PutString("Message", message);

            BasicDialogFragment fragment = new BasicDialogFragment
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

            TextView textViewExplanation = view.FindViewById<TextView>(Resource.Id.textView_explanation);
            textViewExplanation.Text = Arguments.GetString("Message");

            MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
            builder.SetTitle(Arguments.GetString("Title"));
            builder.SetPositiveButton(Android.Resource.String.Ok, (sender, args) => { Dismiss(); });
            builder.SetView(view);
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