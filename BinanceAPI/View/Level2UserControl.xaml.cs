using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BinanceAPI.View
{
    public partial class Level2UserControl : UserControl
    {
        private bool FilterFlag = false;

        public Level2UserControl()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
            SelectorNew1.SelectedItem = Selection.SelectedSymbol;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!FilterFlag)
            {
                FilterPanel.Visibility = Visibility.Visible;
                FilterFlag = true;
                SelectorNew.Margin = new Thickness(0, 55, -34, 0);
                SelectorNew.Height = 376;
            }
            else
            {
                FilterPanel.Visibility = Visibility.Hidden;
                FilterFlag = false;
                SelectorNew.Margin = new Thickness(0, 35, -34, 0);
                SelectorNew.Height = 402;
            }
        }

        private void SizeFilterBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(SizeFilterBox.Text == string.Empty)
            {
                SizeFilterBox.Text = "0";
            }
        }
    }
}
