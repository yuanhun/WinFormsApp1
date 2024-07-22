using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

using System.IO;
using System.Threading.Tasks;
using YourNamespace;



namespace WinFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            if (!IsRunAsAdministrator())
            {
                RestartAsAdministrator();
                return;
            }

            string apiUrl = "https://api.github.com/repos/yuanhun/WinFormsApp1/releases/latest";
            string lastCheckFilePath = "last_update_check.txt";
            string downloadDirectory = "downloads";

            if (!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            // 启动一个任务来检查更新，而不影响程序启动
            Task.Run(() => CheckForUpdatesAsync(apiUrl, lastCheckFilePath, downloadDirectory));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static async Task CheckForUpdatesAsync(string apiUrl, string lastCheckFilePath, string downloadDirectory)
        {
            AutoUpdaterService autoUpdater = new AutoUpdaterService(apiUrl, lastCheckFilePath, downloadDirectory);
            await autoUpdater.CheckForUpdatesAsync();
        }



        private static bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static void RestartAsAdministrator()
        {
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            var startInfo = new ProcessStartInfo(exeName)
            {
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"This application requires elevated privileges to run. Error: {ex.Message}");
            }
            Application.Exit();
        }
    }
}
