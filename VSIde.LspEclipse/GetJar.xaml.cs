using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;

namespace LspEclipse
{
    public partial class GetJar : Window
    {
        public GetJar()
        {
            InitializeComponent();
        }

        private string _uri;
        private string _destination_location;
        private string _decompressed_location;

        public void Download(string uri, string destination_location, string decompressed_location)
        {
            _uri = uri;
            _destination_location = destination_location;
            _decompressed_location = decompressed_location;
            Thread thread = new Thread(() =>
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged +=
                    new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri(uri), destination_location);
            });
            thread.Start();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                //label.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
                pbStatus.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (System.IO.Directory.Exists(_decompressed_location))
                    System.IO.Directory.Delete(_decompressed_location, true);
                FileInfo file_to_decompress = new FileInfo(_destination_location);
                using (FileStream originalFileStream = file_to_decompress.OpenRead())
                {
                    string gzArchiveName = file_to_decompress.FullName;
                    Stream inStream = File.OpenRead(gzArchiveName);
                    Stream gzipStream = new GZipInputStream(inStream);
                    TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
                    tarArchive.ExtractContents(_decompressed_location);
                    tarArchive.Close();
                    gzipStream.Close();
                    inStream.Close();
                }
                this.DialogResult = true;
            });
        }
    }
}
