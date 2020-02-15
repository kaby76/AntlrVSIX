namespace Logger
{
    using System;
    using System.Net.Http;

    public class Log
    {
        public static void Notify(string message)
        {
            var options = Options.Option.GetBoolean("OptInLogging");
            if (options)
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
