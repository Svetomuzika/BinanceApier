using BinanceAPI.Model;
using BinanceAPI.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;

namespace BinanceAPI.View
{
    public partial class Level2UserControl : UserControl
    {
        public Level2UserControl()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Selection.SelectedSymbol;
            SelectorNew1.SelectedItem = Selection.SelectedSymbol;

            
            //var a = new MainViewModel();
           //var b = a.AllOrdersAsks;



            //Binding myBinding = new Binding
            //{
            //    Source = b[0].OrderQPrice,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //};

            //SizeAsks.SetBinding(ItemsControl.ItemsSourceProperty, myBinding);
            //SizeAsks.Text = b[0].OrderQPrice.ToString();


        }
    }
}
