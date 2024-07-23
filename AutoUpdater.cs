using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YourNamespace
{
    public class AutoUpdaterService
    {
        private readonly string apiUrl;
        private readonly string lastCheckFilePath;
        private readonly string downloadDirectory;

        public AutoUpdaterService(string apiUrl, string lastCheckFilePath, string downloadDirectory)
        {
            this.apiUrl = apiUrl;
            this.lastCheckFilePath = lastCheckFilePath;
            this.downloadDirectory = downloadDirectory;
        }

        public async Task CheckForUpdatesAsync()
        {
            if (ShouldCheckForUpdates())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
                        var response = await client.GetStringAsync(apiUrl);
                        var releaseInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<GitHubRelease>(response);

                        //// 打印调试信息以了解 API 响应内容
                        //Debug.WriteLine($"API Response: {response}");
                        //Debug.WriteLine($"Release Tag: {releaseInfo.tag_name}");

                        // 移除 "v" 前缀（如果有）
                        string versionString = releaseInfo.tag_name.StartsWith("v") ? releaseInfo.tag_name.Substring(1) : releaseInfo.tag_name;

                        if (Version.TryParse(versionString, out Version latestVersion))
                        {
                            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                            if (latestVersion > currentVersion)
                            {
                                var latestAsset = releaseInfo.assets[0];
                                var downloadUrl = latestAsset.browser_download_url;


                                try
                                {
                                    //Debug.WriteLine($"AAAAAA: {downloadUrl}");
                                    string downloadPath = Path.Combine(downloadDirectory, latestAsset.name);
                                    await DownloadFileAsync(downloadUrl, downloadPath);



                                    if (new FileInfo(downloadPath).Length > 200) // 检查文件是否大于200B
                                    {
                                        var result = MessageBox.Show(
                                        $"下载新版本成功。是否更新",
                                        "下载新版本成功",
                                        MessageBoxButtons.YesNo
                                    );
                                    if (result == DialogResult.Yes)
                                    {
                                        // 启动更新.bat文件并以管理员身份运行
                                        string batFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "更新.bat");
                                        if (File.Exists(batFilePath))
                                        {
                                            var startInfo = new ProcessStartInfo
                                            {
                                                FileName = batFilePath,
                                                UseShellExecute = true,
                                                Verb = "runas" // 以管理员身份运行
                                            };
                                            Process.Start(startInfo);
                                            UpdateLastCheckTime();
                                        }
                                        else
                                        {
                                            MessageBox.Show("找不到更新.bat文件。");
                                        }

                                        // 确保应用程序退出
                                        Environment.Exit(0);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("下载的文件无效（小于200B）。");

                                        // 确保应用程序退出
                                        Environment.Exit(0);
                                    }



                                }
                                catch (Exception ex)
                                {
                                    ShowUpdateFailedMessage(ex.Message);
                                }
                            }
                            else
                            {
                                Debug.WriteLine("已经是最新版本。");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"无法解析版本号: {releaseInfo.tag_name}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"解析版本号失败: {ex.Message}");
                }
                UpdateLastCheckTime();
            }
            else
            {
                Debug.WriteLine("今天已经检查过更新。");
            }
        }

        private bool ShouldCheckForUpdates()
        {
            if (File.Exists(lastCheckFilePath))
            {
                string lastCheckTimeStr = File.ReadAllText(lastCheckFilePath);
                if (DateTime.TryParse(lastCheckTimeStr, out DateTime lastCheckTime))
                {
                    return (DateTime.Now - lastCheckTime).TotalDays >= 1;
                }
            }
            return true;
        }

        private void UpdateLastCheckTime()
        {
            File.WriteAllText(lastCheckFilePath, DateTime.Now.ToString());
        }

        private async Task DownloadFileAsync(string url, string outputPath)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        private void ShowUpdateFailedMessage(string errorMessage)
        {
            var result = MessageBox.Show(
                $"由于网络问题，应用程序更新失败, 失败错误码: {errorMessage}。 您要打开备用下载地址吗？",
                "更新失败",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error
            );

            if (result == DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.cnblogs.com/123e/p/18317180",
                    UseShellExecute = true
                });
            }
        }

        private class GitHubRelease
        {
            public string tag_name { get; set; }
            public Asset[] assets { get; set; }
        }

        private class Asset
        {
            public string name { get; set; }
            public string browser_download_url { get; set; }
        }
    }
}
