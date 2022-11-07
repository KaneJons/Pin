using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Taskbar;


namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
              btn3_Click(this, new RoutedEventArgs());
          //  Visibility = Visibility.Hidden;
        }
        public RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\TeslaBrowser");
        public RegistryKey KeyEdge = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\IEXPLORE.EXE");
        public string url = "http://";
            
            string w = "";
            string privat = "";
            string defaultbrowser = Registry.CurrentUser
                .OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice")
                .GetValue("ProgId")
                .ToString();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            JumpList jumpList = JumpList.CreateJumpList();
            
            JumpListCustomCategory category = new JumpListCustomCategory("Экспресс-панель");
            
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

            if (key != null)
            {
                if (defaultbrowser != null)
                {
                    w = Registry.ClassesRoot.OpenSubKey($@"{defaultbrowser}\Shell\Open\Command")
                         .GetValue("").ToString();
                    {
                        string keys = ".exe";
                        int pos = w.IndexOf(keys) + keys.Length;
                        if (pos >= keys.Length)
                            w = w.Replace(w.Substring(pos), "");
                        w = w.Remove(0, 1);
                    }
                }
                else
                {
                    w = KeyEdge.GetValue("").ToString();
                }

            }
            else
            {
                w = key.GetValue("DisplayIcon").ToString();
            }
            Process.Start(w);
        }

    }


}
