namespace Logger
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;

    public class Log
    {
        static string home = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        private static ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private static bool done = false;

        // Call only from client.
        public static void CleanUpLogFile()
        {
            if (done) return;
            done = true;
            var log = home
               + System.IO.Path.DirectorySeparatorChar
               + ".antlrlog";
            cacheLock.EnterWriteLock();
            try
            {
                File.Delete(log);
                using (StreamWriter w = File.AppendText(log))
                {
                    w.WriteLine("Logging for Antlrvsix started "
                        + DateTime.Now.ToString());
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

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
