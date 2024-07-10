using System;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace WinFormsApp1
{
    public class RegistryManager
    {
        private const string DotNetCoreKey = @"SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.NETCore.App";
        private const string RegistryKeyPath = @"Software\miHoYo\绝区零"; // 修改为你的注册表路径
        private const string RegistryValueName = "MIHOYOSDK_ADL_PROD_CN_h3123967166"; // 修改为你的注册表值名称
        private const string SubFolder = "UserData"; // 修改为你的子文件夹名称
        private readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public bool IsDotNet6RuntimeInstalled()
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(DotNetCoreKey))
                {
                    if (key == null) return false;

                    foreach (var version in key.GetSubKeyNames())
                    {
                        if (version.StartsWith("6.0"))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking .NET runtime: {ex.Message}");
            }
            return false;
        }

        public void ReadRegistryAndSaveToJsonFile(string fileName)
        {
            try
            {
                // 创建存放文件的目录
                string directoryPath = Path.Combine(baseDirectory, SubFolder);
                Directory.CreateDirectory(directoryPath);

                // 完整的文件路径
                string filePath = Path.Combine(directoryPath, fileName);

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                {
                    if (key != null)
                    {
                        byte[] registryValue = key.GetValue(RegistryValueName) as byte[];

                        if (registryValue != null && registryValue.Length > 0)
                        {
                            string base64String = Convert.ToBase64String(registryValue);
                            string jsonContent = JsonConvert.SerializeObject(base64String, Formatting.Indented);

                            File.WriteAllText(filePath, jsonContent);
                            Console.WriteLine($"注册表值已成功保存到 {filePath}");
                        }
                        else
                        {
                            Console.WriteLine("注册表值为空或不存在。");
                        }
                    }
                    else
                    {
                        Console.WriteLine("指定的注册表项不存在。");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading registry and saving to JSON: {ex.Message}");
            }
        }

        public void SetRegistryValueFromJsonFile(string fileName)
        {
            try
            {
                // 创建存放文件的目录
                string directoryPath = Path.Combine(baseDirectory, SubFolder);

                // 完整的文件路径
                string filePath = Path.Combine(directoryPath, fileName);

                string jsonContent = File.ReadAllText(filePath);
                string base64String = JsonConvert.DeserializeObject<string>(jsonContent);
                byte[] registryValue = Convert.FromBase64String(base64String);

                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                {
                    if (key != null)
                    {
                        key.SetValue(RegistryValueName, registryValue, RegistryValueKind.Binary);
                        Console.WriteLine("注册表值已成功更新。");
                    }
                    else
                    {
                        Console.WriteLine("无法打开或创建指定的注册表项。");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting registry value from JSON file: {ex.Message}");
            }
        }
    }
}
