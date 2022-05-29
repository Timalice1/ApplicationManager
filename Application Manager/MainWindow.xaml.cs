using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Application_Manager {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        List<Info> installedApps = new List<Info>();

        private void Window_Loaded(object sender, RoutedEventArgs e) {

            //Get list of installed apps from Windows registry
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey)) {
                foreach (string skName in rk.GetSubKeyNames()) {
                    using (RegistryKey sk = rk.OpenSubKey(skName)) {
                        try {

                            var path = sk.GetValue("InstallLocation") as string;
                            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) {
                                continue;
                            }

                            //Get app size
                            var files = Directory.GetFiles(sk.GetValue("InstallLocation").ToString(), "*", SearchOption.AllDirectories);
                            long size = 0;
                            foreach (var file in files)
                                size += new FileInfo(file).Length;

                            //Get install date
                            var date = sk.GetValue("InstallDate") as string;
                            var installDate = DateTime.ParseExact(date, "yyyyMMdd", null);
                            string _date = installDate.ToString("yyyy.MM.dd");

                            installedApps.Add(
                                new Info {
                                    //Get app info from registry
                                    Name = sk.GetValue("DisplayName") as string,
                                    Version = sk.GetValue("DisplayVersion") as string,
                                    Publisher = sk.GetValue("Publisher") as string ,
                                    InstallationDate = _date,
                                    UninstallString = sk.GetValue("UninstallString") as string,
                                    Size = (size / 1048576).ToString() + " MB"
                                });
                        }
                        catch (Exception ex) {
                        }
                    }
                }
                //Show app list on form
                appList.ItemsSource = installedApps;
            }

        }

        private Info app;
        //Start new process of uninstalling app
        private void btnUninstall_Click(object sender, RoutedEventArgs e) {
            var res = MessageBox.Show($"You realy want remove \"{app.Name}\" from your PC?", "Delete", MessageBoxButton.OKCancel);
            if(res == MessageBoxResult.OK) {
                Process.Start(app.UninstallString);
                installedApps.RemoveAt(appList.SelectedIndex);
                appList.Items.RemoveAt(appList.SelectedIndex);
                btnUninstall.IsEnabled = false;
            }
        }

        class Info {
            public string Name { get; set; }
            public string Version { get; set; }
            public string Publisher { get; set; }
            public string InstallationDate { get; set; }
            public string UninstallString { get; set; }
            public string Size { get; set; }
        }

        private void appList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            app = installedApps[appList.SelectedIndex];

            if (!string.IsNullOrEmpty(app.UninstallString)) {
                btnUninstall.IsEnabled = true;
               
            }else btnUninstall.IsEnabled = false;
        }
    }
}
