using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using MyBroadcastReciever.BroadCast;
using System;

namespace MyBroadcastReciever
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView tv;
        BroadCastHeadSets brHeadSet;
        SMSReceiver sms_receiver;
        Button btnRegister, btnUnregister;
        TextView tvSMS;

        Android.Content.ISharedPreferences mySHP;
        bool IsRegistered;

        private const string TAG = "MYAPP";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //Checking if SMS Permissions Granted
            if ((ApplicationContext.CheckCallingOrSelfPermission(Android.Manifest.Permission.ReceiveSms) !=
                                                                                Android.Content.PM.Permission.Granted))
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.ReceiveSms }, 1);
            }
            MyInitilize();
            Log.Debug(TAG, "OnCreate: IsRegistered " + IsRegistered.ToString());           
        }

        private void MyInitilize()
        {
            tv = FindViewById<TextView>(Resource.Id.tvHeadsetState);
            brHeadSet = new BroadCastHeadSets(tv);

            mySHP = this.GetSharedPreferences("ReceiverStatus", Android.Content.FileCreationMode.Private);
            IsRegistered = mySHP.GetBoolean("ReceiverStatus", false);
            btnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            btnUnregister = FindViewById<Button>(Resource.Id.btnUnRegister);

            tv = FindViewById<TextView>(Resource.Id.tvInSMSText);
            sms_receiver = new SMSReceiver(tv);

            btnRegister.Click += BtnRegister_Click;
            btnUnregister.Click += BtnUnregister_Click;     
            if (IsRegistered)
            {
                ButtonDisable(btnRegister);
                ButtonEnable(btnUnregister);
            }
            else
            {
                ButtonDisable(btnUnregister);
                ButtonEnable(btnRegister);
            }
            //Showing the SMS body message
            if (Intent.HasExtra("info"))
            {
                string info = Intent.GetStringExtra("info");
                tv.Text = info;
            }
        }

        private void ButtonEnable(Button btn)
        {
            btn.SetTextColor(Color.Black);
            btn.SetBackgroundColor(Color.ParseColor("#b3b3b3"));
            btn.Enabled = true;
        }

        private void ButtonDisable(Button btn)
        {
            btn.SetTextColor(Color.ParseColor("#ffffff"));
            btn.SetBackgroundColor(Color.ParseColor("#cccccc"));
            btn.Enabled = false;
        }

        private void BtnUnregister_Click(object sender, EventArgs e)
        {
            if (IsRegistered)
            {
                IsRegistered = false;
                var editor = mySHP.Edit();
                editor.PutBoolean("ReceiverStatus", IsRegistered);
                editor.Commit();
                UnregisterReceiver(sms_receiver);
                ButtonEnable(btnRegister);
                ButtonDisable(btnUnregister);
                Log.Debug(TAG, "BTN UnRegister: IsRegistered " + IsRegistered.ToString());
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {            
            if (!IsRegistered)
            {
                IsRegistered = true;
                var editor = mySHP.Edit();
                editor.PutBoolean("ReceiverStatus", IsRegistered);
                editor.Commit();
                ButtonEnable(btnUnregister);
                ButtonDisable(btnRegister);
                RegisterReceiver(sms_receiver, new
                IntentFilter("android.provider.Telephony.SMS_RECEIVED"));
                Log.Debug(TAG, "BTN Registered: IsRegistered " + IsRegistered.ToString());
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(brHeadSet, 
                new Android.Content.IntentFilter(Intent.ActionHeadsetPlug));

            if (IsRegistered)
            {
                RegisterReceiver(sms_receiver, new IntentFilter("android.provider.Telephony.SMS_RECEIVED"));
                Log.Debug(TAG, "OnResume Registered: IsRegistered " + IsRegistered.ToString());
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(brHeadSet);

            if (IsRegistered)
            {
                UnregisterReceiver(sms_receiver);
                Log.Debug(TAG, "OnPause Unregistered: IsRegistered " + IsRegistered.ToString());
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (IsRegistered)
            {
                IsRegistered = false;
                var editor = mySHP.Edit();
                editor.PutBoolean("ReceiverStatus", IsRegistered);
                editor.Commit();
                UnregisterReceiver(sms_receiver);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == 1)
            {
                if (grantResults[0] != Android.Content.PM.Permission.Granted)
                {
                    //Toast.MakeText(this, "Oh No!!! Permission Denied. App will EXIT", ToastLength.Long).Show();
                    //Two examples to exit from App
                    //System.Environment.Exit(0);
                    //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    tv.Text = "SMS SEND/RECEIVE Permission denied, application may not work correctly!";
                }
            }
        }
    }
}