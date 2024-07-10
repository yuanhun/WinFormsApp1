using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using IWshRuntimeLibrary; // ���� Windows Script Host Object Model

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
                MessageBox.Show("���ļ��� 'UserData' �����ڡ�");
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
                MessageBox.Show("�ļ�·����Ч�򲻴��ڡ�");
                return;
            }

            await Task.Delay(200);
            try
            {
                // ����һ���µ� ProcessStartInfo ���󣬲����� Verb ����
                ProcessStartInfo startInfo = new ProcessStartInfo(filePath)
                {
                    UseShellExecute = true
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʧ�ܣ�{ex.Message}");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                MessageBox.Show("��������Ч���ļ�·����");
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
                button2.Text = "[����ɹ�]";
                PopulateComboBoxWithFiles();
                textBox2.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������{ex.Message}");
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            button2.Text = "����";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                string fileName = Path.GetFileName(selectedFilePath);

                if (fileName != "ZenlessZoneZero.exe")
                {
                    MessageBox.Show("�ļ�������ȷ����ѡ�� ZenlessZoneZero.exe");
                    return;
                }
                // ��ȡѡ���ļ��ĸ�Ŀ¼
                string rootDirectory = Path.GetDirectoryName(selectedFilePath);

                // ��ȡ��ǰ����ĸ�Ŀ¼
                string appRootDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // ������ݷ�ʽ
                CreateShortcut(appRootDirectory, rootDirectory, "��Ϸ����");




                string directoryA = Path.GetDirectoryName(selectedFilePath);
                string newFilePath = Path.Combine(directoryA, "ZenlessZoneZero_Data\\Plugins\\x86_64\\PCGameSDK.dll");

                if (!System.IO.File.Exists(newFilePath))
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string zipPath = Path.Combine(baseDirectory, "ZxZxxZxxx.zip");

                    if (!System.IO.File.Exists(zipPath))
                    {
                        MessageBox.Show("ZIP �ļ������ڣ�");
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
                                    // ����Ŀ¼
                                    Directory.CreateDirectory(destinationPath);
                                    continue;
                                }

                                // ����Ŀ¼�ṹ
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                                // ǿ�Ƹ����ļ�
                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                        MessageBox.Show("˫��ת��������ɹ���");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"��ѹʧ�ܣ�{ex.Message}");
                    }
                }

                label5.Text = selectedFilePath;
                iniFile.Write(iniSection, filePathKey, selectedFilePath);
            }
        }
        // ������ݷ�ʽ�ķ���
        private void CreateShortcut(string shortcutDirectory, string targetDirectory, string shortcutName)
        {
            string shortcutPath = Path.Combine(shortcutDirectory, $"{shortcutName}.lnk");

            // ���� WScript.Shell ����
            WshShell shell = new WshShell();
            // ������ݷ�ʽ
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetDirectory; // ����Ŀ��·��
            shortcut.WorkingDirectory = targetDirectory; // ���ù���Ŀ¼
            shortcut.Save(); // �����ݷ�ʽ
        }



        private void button4_Click(object sender, EventArgs e)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string targetIniPath = label1.Text == "B��"
                ? Path.Combine(baseDirectory, "�ٷ�config.ini")
                : Path.Combine(baseDirectory, "B��config.ini");

            label1.Text = label1.Text == "B��" ? "�ٷ�" : "B��";

            if (System.IO.File.Exists(targetIniPath))
            {
                try
                {
                    // ��ȡINI�ļ��е�ֵ
                    string savedFilePath = iniFile.Read(iniSection, filePathKey);
                    // ��ȡ��Ŀ¼
                    string directoryA = Path.GetDirectoryName(savedFilePath);
                    string destinationPath = Path.Combine(directoryA, "config.ini");

                    // ǿ�Ƹ��Ʋ������ļ�
                    System.IO.File.Copy(targetIniPath, destinationPath, true);

                    iniFile.Write(iniSection, "SWITCH_SERVER", label1.Text);
                    //MessageBox.Show($"�ɹ��л��� {label1.Text}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"�����ļ�ʧ�ܣ�{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("�����ļ������ڣ�");
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
