using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroadcastReciever.BroadCast
{
    [BroadcastReceiver]
    public class BroadCastHeadSets : BroadcastReceiver
    {
        TextView tv;

        public BroadCastHeadSets() { }
        public BroadCastHeadSets(TextView tv) { this.tv = tv; }

        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
            //Log.Debug(TAG, "HEADSET RECEIVER GOT MESSAGE");
            if (intent.Action == Intent.ActionHeadsetPlug)
            {
                int state = intent.GetIntExtra("state", -1);
                int microphone = intent.GetIntExtra("microphone", 0);
                switch (state)
                {
                    case 0:
                        tv.Text = "headset unplugged";
                        Toast.MakeText(context, "headset unplugged", ToastLength.Short).Show();
                        break;
                    case 1:
                        tv.Text = "headset plugged, with microphone + " + (microphone == 1);
                        Toast.MakeText(context, tv.Text, ToastLength.Short).Show();
                        break;
                    default:
                        tv.Text = "unknown state ";
                        Toast.MakeText(context, "unknown state ", ToastLength.Short).Show();
                        break;
                }
            }
        }
    }
}