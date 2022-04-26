using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Application_Manager {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        List<string> pathes = new List<string>();
        string uninstallFile = "";

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey)) {
                foreach (string skName in rk.GetSubKeyNames()) {
                    using (RegistryKey sk = rk.OpenSubKey(skName)) {
                        try {
                            var name = sk.GetValue("DisplayName") as string;
                            var path = sk.GetValue("InstallLocation") as string;
                            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) {
                                continue;
                            }

                            pathes.Add(path);

                            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
                            appList.Items.Add(name);
                        }
                        catch (Exception ex) { }
                    }
                }
            }
        }

        private void appList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                var index = appList.SelectedIndex;
                var files = Directory.GetFiles(pathes[index], "*.*", SearchOption.AllDirectories);
                string name;
                foreach (var file in files) {
                    if (file.Contains("unins000.exe")) {
                        btnUninstall.IsEnabled = true;
                        uninstallFile = file;
                        return;
                    }else btnUninstall.IsEnabled = false;
                }
            }catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUninstall_Click(object sender, RoutedEventArgs e) {
            var res = MessageBox.Show($"You realy want uninstall \"{appList.SelectedItem}\"?", "Uninstall", MessageBoxButton.OKCancel);
            if(res == MessageBoxResult.OK) {
                Process.Start(uninstallFile);
                pathes.RemoveAt(appList.SelectedIndex);
                appList.Items.RemoveAt(appList.SelectedIndex);
                btnUninstall.IsEnabled = false;
            }
        }
    }
}
