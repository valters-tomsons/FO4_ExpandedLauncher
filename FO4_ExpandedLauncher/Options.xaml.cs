using FO4_ExpandedLauncher.Domain;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FO4_ExpandedLauncher
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Page
    {
        public Options()
        {
            InitializeComponent();

            //Back button mouse event handler
            BackButton.MouseLeftButtonDown += new MouseButtonEventHandler(BackButton_Click);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).HideOptions();
            ExpandedINI.WriteOptions(Convert.ToBoolean(CustomCheckbox.IsChecked), CustomEXE.Text);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            
            if(obj.IsChecked == true)
            {
                CustomEXE.IsEnabled = true;
            }
            else
            {
                CustomEXE.IsEnabled = false;
            }
        }

        public void RefreshOptions()
        {
            Tuple<bool, string> e = ExpandedINI.GetOptions();
            CustomCheckbox.IsChecked = e.Item1;
            CustomEXE.Text = e.Item2;
        }
    }
}
