namespace Logger
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;

    public class Log
    {
        static string home = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

        public static void WriteData(string message)
        {
            var log = home
               + System.IO.Path.DirectorySeparatorChar
               + ".antlrlog";
            cacheLock.EnterWriteLock();
            try
            {
                using (StreamWriter w = File.AppendText(log))
                {
                    w.WriteLine(message);
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        private static ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public static void Notify(string message)
        {
            WriteData(message);
            bool options = Options.Option.GetBoolean("OptInLogging");
            if (options)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage result = httpClient.GetAsync("http://domemtech.com/home/db?" + message).Result;
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
