using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBroadcastReciever.BroadCast
{
    [BroadcastReceiver]
    public class SMSReceiver : BroadcastReceiver
    {
        private TextView tvSMS;
        private const string TAG = "MYAPP";

        public SMSReceiver() { }
        public SMSReceiver(TextView tv) { this.tvSMS = tv; }
        public override void OnReceive(Context context, Intent intent)
        {
            Log.Debug(TAG, "New SMS Arrived");
            GetSMSAction(context, intent);
        }

        private void GetSMSAction(Context context, Intent intent)
        {
            string strMessage = "", sender = "";
            SmsMessage[] msgs;                    //using Android.Telephony;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                msgs = Telephony.Sms.Intents.GetMessagesFromIntent(intent); //smsMessage = msgs[0];
                foreach (SmsMessage msg in msgs)
                {
                    sender = msg.OriginatingAddress;
                    strMessage += "SMS From: " + msg.OriginatingAddress;
                    strMessage += "\n";
                    strMessage += msg.MessageBody;
                    strMessage += "\n";
                }
            }
            else //Old SDK versions
            {
                if (intent.HasExtra("pdus"))
                {
                    var smsArray = (Java.Lang.Object[])intent.Extras.Get("pdus");
                    foreach (var item in smsArray)
                    {
                        var sms = SmsMessage.CreateFromPdu((byte[])item);
                        sender = sms.OriginatingAddress;
                        strMessage += "SMS From: " + sms.OriginatingAddress;
                        strMessage += "\n";
                        strMessage += sms.MessageBody;
                        strMessage += "\n";
                    }
                }
            }
            
            tvSMS.Text = strMessage; //Geting an exception

            Log.Debug(TAG, $"OriginatingAddress: {sender}");

            if (strMessage.Length > 0)
            {
                Intent smsIntent = new Intent(context, typeof(MainActivity));
                smsIntent.PutExtra("info", strMessage);
                smsIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(smsIntent);
            }           
        }
    }
}