using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.IO;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
              btn3_Click(this, new RoutedEventArgs());
            Visibility = Visibility.Hidden;
        }
        public RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\TeslaBrowser");
        public RegistryKey KeyEdge = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\IEXPLORE.EXE");
        public string url = "http://";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            JumpList jumpList = JumpList.CreateJumpList();
            
            JumpListCustomCategory category = new JumpListCustomCategory("Экспресс-панель");

            string w="";
            string privat="";
            string defaultbrowser = Registry.CurrentUser
                .OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice")
                .GetValue("ProgId")
                .ToString();

            
            if (key == null)
            {
                if (defaultbrowser != null)
                {
                   w= Registry.ClassesRoot.OpenSubKey($@"{defaultbrowser}\Shell\Open\Command")
                        .GetValue("").ToString();
                    {
                        string keys = ".exe";
                        int pos = w.IndexOf(keys) + keys.Length;
                        if (pos >= keys.Length)
                            w = w.Replace(w.Substring(pos), "");
                        w= w.Remove(0, 1);
                    }
                    switch (defaultbrowser)
                    {
                        case "FirefoxURL":
                            privat = "-private-window ";
                            break;
                        case "ChromeHTML":
                            privat = "-incognito ";
                            break;
                        default:
                            privat = "-private ";
                            break;
                    }
                }
                else
                {
                    w = KeyEdge.GetValue("").ToString();
                    privat = "-private ";
                }
               
            }
            else
            {
               w = key.GetValue("DisplayIcon").ToString();
                privat = "-incognito ";
            }

            category.AddJumpListItems(
                new JumpListLink(w, "Создать новое окно") { Arguments = url },
                new JumpListLink(w, "Создать приватное окно") { Arguments = privat + url },
                new JumpListLink(w,"Открыть браузер") ); 
            
            jumpList.AddCustomCategories(category);
            jumpList.Refresh();
            this.Close();
        }
    
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (key == null)
            {
                using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
                {
                    object progIdValue = userChoiceKey.GetValue("Progid");

                    switch (progIdValue.ToString())
                    {
                        case "FirefoxURL":
                            Process.Start("firefox.exe");
                            break;
                        case "ChromeHTML":
                            Process.Start("chrome.exe");
                            break;
                        case "Opera GXStabl":
                            Process.Start("opera.exe");
                            break;
                        case "Opera.Protocol":
                            Process.Start("opera.exe");
                            break;
                        case "YandexHTML":
                            Process.Start("browser.exe");
                            break;
                        default:
                            Process.Start("iexplore.exe");
                            break;
                    }
                }
            }
            else
            {
                string w = key.GetValue("DisplayIcon").ToString();
                Process.Start(new ProcessStartInfo { FileName = w });
            }
           
        }

    }


}
