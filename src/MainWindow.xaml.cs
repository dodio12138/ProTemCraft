using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;

namespace src
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // 启用窗体的拖放功能
            AllowDrop = true;
            DragEnter += MainWindow_DragEnter;
            Drop += MainWindow_DragDrop;
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            // 检查拖入的内容是否为文件夹
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && Directory.Exists(files[0]))
                {
                    e.Effects = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effects = DragDropEffects.None;
        }

        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            // 获取拖入的文件夹路径
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var folderPath = files[0];
            string parentDirectory = Path.GetDirectoryName(folderPath);

            // 遍历文件夹中的所有文件
            var filesInFolder = Directory.EnumerateFileSystemEntries(folderPath, "*", SearchOption.AllDirectories);

            // 创建批处理文件并写入复制命令
            var batFilePath = Path.Combine(parentDirectory, "copy_files.bat");

            using (var writer = new StreamWriter(batFilePath))
            {
                writer.WriteLine($"@echo off");
                writer.WriteLine($"chcp 65001 > nul");
                foreach (var filePath in filesInFolder)
                {
                    //string parentPath = Path.GetDirectoryName(filePath);

                    if (File.Exists(filePath))
                    {
                        string path = filePath.Replace(parentDirectory, string.Empty);

                        string createFolderCommand = $"echo. > .{path}";
                        writer.WriteLine(createFolderCommand);
                    }
                    else if (Directory.Exists(filePath))
                    {
                        string path = filePath.Replace(parentDirectory, string.Empty);

                        string createFolderCommand = $"mkdir .{path}";
                        writer.WriteLine(createFolderCommand);
                    }
                    
                }
            }

            MessageBox.Show("批处理文件已创建成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void App_DragDrop(string appPath)
        {
            // 获取拖入的文件夹路径
            var folderPath = appPath;
            string parentDirectory = Path.GetDirectoryName(folderPath);

            // 遍历文件夹中的所有文件
            var filesInFolder = Directory.EnumerateFileSystemEntries(folderPath, "*", SearchOption.AllDirectories);

            // 创建批处理文件并写入复制命令
            var batFilePath = Path.Combine(parentDirectory, "copy_files.bat");

            using (var writer = new StreamWriter(batFilePath))
            {
                writer.WriteLine($"@echo off");
                writer.WriteLine($"chcp 65001 > nul");
                foreach (var filePath in filesInFolder)
                {
                    //string parentPath = Path.GetDirectoryName(filePath);

                    if (File.Exists(filePath))
                    {
                        string path = filePath.Replace(parentDirectory, string.Empty);

                        string createFolderCommand = $"echo. > .{path}";
                        writer.WriteLine(createFolderCommand);
                    }
                    else if (Directory.Exists(filePath))
                    {
                        string path = filePath.Replace(parentDirectory, string.Empty);

                        string createFolderCommand = $"mkdir .{path}";
                        writer.WriteLine(createFolderCommand);
                    }

                }
            }

            MessageBox.Show("批处理文件已创建成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length > 0)
            {
                string folderPath = e.Args[0];


                MainWindow mainWindow1 = new MainWindow();

                mainWindow1.App_DragDrop(folderPath);
                // 执行你的逻辑操作
                //Console.WriteLine($"拖放的文件夹路径：{folderPath}");

                // 在这里调用你的其他函数或类
                // ...

                // 等待应用程序关闭
                Current.Shutdown();
            }
            else
            {
                MainWindow mainWindow2 = new MainWindow();
                mainWindow2.Show();
            }
        }
    }
}
