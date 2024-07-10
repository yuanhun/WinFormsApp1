using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using IWshRuntimeLibrary; // 引用 Windows Script Host Object Model

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly IniFile iniFile;
        private const string iniPath = "config.ini";
        private const string iniSection = "Settings";
        private const string filePathKey = "FilePath";
        private readonly RegistryManager manager = new RegistryManager();

        public Form1()
        {
            InitializeComponent();

            iniFile = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iniPath));

            LoadSwitchServer();
            PopulateComboBoxWithFiles();
            LoadSavedFilePath();
        }

        private void LoadSwitchServer()
        {
            string switchServer = iniFile.Read(iniSection, "SWITCH_SERVER");
            if (!string.IsNullOrEmpty(switchServer))
            {
                label1.Text = switchServer;
            }
        }

        private void PopulateComboBoxWithFiles()
        {
            string subFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");

            if (!Directory.Exists(subFolderPath))
            {
                MessageBox.Show("子文件夹 'UserData' 不存在。");
                return;
            }

            comboBox1.Items.Clear();
            foreach (var file in Directory.GetFiles(subFolderPath))
            {
                comboBox1.Items.Add(Path.GetFileName(file));
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {   
            if (!string.IsNullOrEmpty(comboBox1.SelectedItem as string))
            {
                manager.SetRegistryValueFromJsonFile(comboBox1.SelectedItem.ToString());
            }
            //MessageBox.Show(filePath);/////////////////////////////////////////////////////////////

            string filePath = iniFile.Read(iniSection, filePathKey);
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                MessageBox.Show("文件路径无效或不存在。");
                return;
            }

            await Task.Delay(200);
            try
            {
                // 创建一个新的 ProcessStartInfo 对象，不设置 Verb 属性
                ProcessStartInfo startInfo = new ProcessStartInfo(filePath)
                {
                    UseShellExecute = true
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动程序失败：{ex.Message}");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                MessageBox.Show("请输入有效的文件路径。");
                return;
            }

            string fileName = textBox2.Text.Trim();
            string subFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
            string filePath = Path.Combine(subFolderPath, fileName);

            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }

            try
            {
                manager.ReadRegistryAndSaveToJsonFile(filePath);
                button2.Text = "[保存成功]";
                PopulateComboBoxWithFiles();
                textBox2.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            button2.Text = "保存";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                string fileName = Path.GetFileName(selectedFilePath);

                if (fileName != "ZenlessZoneZero.exe")
                {
                    MessageBox.Show("文件名不正确，请选择 ZenlessZoneZero.exe");
                    return;
                }
                // 获取选中文件的根目录
                string rootDirectory = Path.GetDirectoryName(selectedFilePath);

                // 获取当前程序的根目录
                string appRootDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // 创建快捷方式
                CreateShortcut(appRootDirectory, rootDirectory, "游戏本体");




                string directoryA = Path.GetDirectoryName(selectedFilePath);
                string newFilePath = Path.Combine(directoryA, "ZenlessZoneZero_Data\\Plugins\\x86_64\\PCGameSDK.dll");

                if (!System.IO.File.Exists(newFilePath))
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string zipPath = Path.Combine(baseDirectory, "ZxZxxZxxx.zip");

                    if (!System.IO.File.Exists(zipPath))
                    {
                        MessageBox.Show("ZIP 文件不存在！");
                        return;
                    }

                    if (!Directory.Exists(directoryA))
                    {
                        Directory.CreateDirectory(directoryA);
                    }

                    try
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                string destinationPath = Path.Combine(directoryA, entry.FullName);
                                if (entry.Name == "")
                                {
                                    // 处理目录
                                    Directory.CreateDirectory(destinationPath);
                                    continue;
                                }

                                // 创建目录结构
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                                // 强制覆盖文件
                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                        MessageBox.Show("双服转换环境搭建成功！");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"解压失败：{ex.Message}");
                    }
                }

                label5.Text = selectedFilePath;
                iniFile.Write(iniSection, filePathKey, selectedFilePath);
            }
        }
        // 创建快捷方式的方法
        private void CreateShortcut(string shortcutDirectory, string targetDirectory, string shortcutName)
        {
            string shortcutPath = Path.Combine(shortcutDirectory, $"{shortcutName}.lnk");

            // 创建 WScript.Shell 对象
            WshShell shell = new WshShell();
            // 创建快捷方式
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetDirectory; // 设置目标路径
            shortcut.WorkingDirectory = targetDirectory; // 设置工作目录
            shortcut.Save(); // 保存快捷方式
        }



        private void button4_Click(object sender, EventArgs e)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string targetIniPath = label1.Text == "B服"
                ? Path.Combine(baseDirectory, "官服config.ini")
                : Path.Combine(baseDirectory, "B服config.ini");

            label1.Text = label1.Text == "B服" ? "官服" : "B服";

            if (System.IO.File.Exists(targetIniPath))
            {
                try
                {
                    // 读取INI文件中的值
                    string savedFilePath = iniFile.Read(iniSection, filePathKey);
                    // 提取根目录
                    string directoryA = Path.GetDirectoryName(savedFilePath);
                    string destinationPath = Path.Combine(directoryA, "config.ini");

                    // 强制复制并覆盖文件
                    System.IO.File.Copy(targetIniPath, destinationPath, true);

                    iniFile.Write(iniSection, "SWITCH_SERVER", label1.Text);
                    //MessageBox.Show($"成功切换到 {label1.Text}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"复制文件失败：{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("配置文件不存在！");
            }
        }


        private void LoadSavedFilePath()
        {
            string savedFilePath = iniFile.Read(iniSection, filePathKey);
            if (!string.IsNullOrEmpty(savedFilePath))
            {
                label5.Text = savedFilePath;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Openbilibili();
        }
        private void Openbilibili()
        {
            string url = "https://space.bilibili.com/3808570";
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open URL: {ex.Message}");
            }
        }

    }

    public class IniFile
    {
        public string Path { get; }

        public IniFile(string iniPath)
        {
            Path = new FileInfo(iniPath).FullName;
        }

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        public string Read(string section, string key)
        {
            var retVal = new System.Text.StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, Path);
            return retVal.ToString();
        }
    }
}
