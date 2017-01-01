using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace luval.android.utils
{
    public static class ActivityExtensions
    {
        public static JToken GetJsonAsset(this Activity a, string fileName)
        {
            var json = default(string);
            using (var reader = new StreamReader(a.Assets.Open(fileName)))
            {
                json = reader.ReadToEnd();
                reader.Close();
            }
            return (JToken)JsonConvert.DeserializeObject(json);
        }

        public static void ExecuteLongTask(this Activity a, Action onExecute, Action onDone, string title, string message)
        {
            var wait = ProgressDialog.Show(a, title, message, true, false);
            new Thread(new ThreadStart(delegate
            {
                onExecute();
                a.RunOnUiThread(() =>
                {
                    wait.Dismiss();
                    onDone();
                });
            })).Start();
        }

        public static void ShowDialogOk(this Activity a, Action onOk, string title, string content)
        {
            var alert = new AlertDialog.Builder(a).Create();
            alert.SetTitle(title);
            alert.SetMessage(content);
            alert.SetButton("OK", (senderAlert, args) => {
                onOk();
            });
            alert.Show();

        }
    }
}