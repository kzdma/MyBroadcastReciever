using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroadcastReciever.BroadCast
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class MyBootReciever : BroadcastReceiver
    {
        private static readonly string TAG = "MYAPP";
        public MyBootReciever() { }

        public override void OnReceive(Context context, Intent intent)
        {
            Log.WriteLine(LogPriority.Debug, TAG, "BootReceiver OnReceive");
            //Log.Debug(TAG,"Version 2 BootReceiver OnReceive");
        }
    }
}