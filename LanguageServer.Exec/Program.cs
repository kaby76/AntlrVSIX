using System;
using System.Threading.Tasks;

namespace LanguageServer.Exec
{
    internal static class Program
    {
        public static async Task Main()
        {
            LanguageServerHost languageServerHost = null;
            try
            {
                languageServerHost = await LanguageServerHost.Create(
                    Console.OpenStandardInput(),
                    Console.OpenStandardOutput());
            }
            catch (Exception ex)
            {
                languageServerHost?.Dispose();
                Console.Error.WriteLine(ex);
                return;
            }

            try
            {
                await languageServerHost.WaitForExit;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return;
            }
            finally
            {
                languageServerHost.Dispose();
            }
        }
    }
}
