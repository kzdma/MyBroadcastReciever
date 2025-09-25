using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using MyBroadcastReciever.BroadCast;
using Android.Content;
using Android.Util;

namespace MyBroadcastReciever
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView tv;
        BroadCastHeadSets brHeadSet;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            tv = FindViewById<TextView>(Resource.Id.tvHeadsetState);
            brHeadSet = new BroadCastHeadSets(tv);

            Log.Debug("MYAPP", "MainActivity OnCreate");
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(brHeadSet, 
                new Android.Content.IntentFilter(Intent.ActionHeadsetPlug));
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(brHeadSet);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}