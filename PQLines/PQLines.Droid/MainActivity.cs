using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace PQLines.Droid
{
    [Activity(Label = "PQLines", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class MainActivity : XFormsApplicationDroid
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // For Acr.UserDialogs
            if (UserDialogs.Instance == null)
            {
                UserDialogs.Init(() => (Activity) Forms.Context);
            }

            Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}