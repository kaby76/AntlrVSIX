namespace Logger
{
    using System;
    using System.Net.Http;
    using System.Windows.Forms;

    public class Log
    {
        public static void Notify(string message)
        {
            var options = Options.POptions.GetInt32("OptInLogging");
            if (options == 0)
            {
                DialogResult result = MessageBox.Show("AntlrVSIX would like to send a stack trace of an exception. Can we do that? (You can change this in the AntlrVSIX options at any time.)",
                    "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //code for Yes
                    Options.POptions.SetInt32("OptInLogging", 1);
                }
                else if (result == DialogResult.No)
                {
                    //code for No
                    Options.POptions.SetInt32("OptInLogging", 2);
                }
                options = Options.POptions.GetInt32("OptInLogging");
            }
            if (options == 1)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    var result = httpClient.GetAsync("http://domemtech.com/home/db?" + message).Result;
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
