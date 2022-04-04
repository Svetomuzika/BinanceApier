using System.Windows;

namespace BinanceAPI
{ 
    public partial class NewWindow : Window
    {
        public NewWindow()
        {
            InitializeComponent();

            SelectorNew.SelectedItem = Slection.SelectedSymbol;
        }
    }
}
