using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
                            //Get App name
                            var name = sk.GetValue("DisplayName") as string;

                            //Get instalation date
                            var date = sk.GetValue("InstallDate") as string;
                            var installDate = DateTime.ParseExact(date, "yyyyMMdd", null);
                            string _date = installDate.ToString("yyyy.MM.dd");

                            //Get path
                            var path = sk.GetValue("InstallLocation") as string;
                            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) {
                                continue;
                            }

                            pathes.Add(path);

                            //var panel = new StackPanel();
                            var grid = new Grid();
                            //var col1 = new ColumnDefinition() /*{ Width = new GridLength(500)}*/;
                            //var col2 = new ColumnDefinition() /*{ Width = new GridLength(200)}*/;
                            //var col3 = new ColumnDefinition();

                            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(500) });
                            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150)});
                            grid.ColumnDefinitions.Add(new ColumnDefinition());

                            //panel.Children.Add(grid);


                            //Add app name
                            var tbName = new TextBlock { Text = name };
                            Grid.SetColumn(tbName,0);

                            //Get folder size
                            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                            long size = 0;
                            foreach (var file in files)
                                size += new FileInfo(file).Length;

                            //Add folder size
                            TextBlock tbSize;
                            if((size / 1048576) > 1024) {
                                tbSize = new TextBlock { Text = ((size / 1048576)/1024).ToString() + " GB" };
                            }
                            else tbSize = new TextBlock { Text = (size / 1048576).ToString() + " MB" };
                            Grid.SetColumn(tbSize,1);

                            var tbDate = new TextBlock { Text = _date };
                            Grid.SetColumn(tbDate, 2);
                            
                            //Add elements to grid
                            grid.Children.Add(tbName);
                            grid.Children.Add(tbSize);
                            grid.Children.Add(tbDate);

                            appList.Items.Add(grid);
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
            var name = (((appList.SelectedItem) as Grid).Children[0] as TextBlock).Text;
            var res = MessageBox.Show($"You realy want uninstall \"{name}\"?",
                "Uninstall", MessageBoxButton.OKCancel);
            if(res == MessageBoxResult.OK) {
                Process.Start(uninstallFile);
                pathes.RemoveAt(appList.SelectedIndex);
                appList.Items.RemoveAt(appList.SelectedIndex);
                btnUninstall.IsEnabled = false;
            }
        }

    }
}
