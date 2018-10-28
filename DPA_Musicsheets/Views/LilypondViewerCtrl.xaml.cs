using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
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

namespace DPA_Musicsheets.Views
{
    /// <summary>
    /// Interaction logic for LilypondViewer.xaml
    /// </summary>
    public partial class LilypondViewerCtrl : UserControl
    {
        public LilypondViewerCtrl()
        {
            InitializeComponent();
        }

		// Sorry about this, but even after searching intensively, I could not find out how to
		// get a reference to either the textbox itself or just its selection, in an event that is automatically passed to the ViewModel.
		// In my KeyHandler, there is no possible way around requiring one of those things, but even our LilypondViewModel
		// normally knows nothing about it. I didn't have enough time to figure out how to do this the MVVM way...
		private void SelectionChanged(object sender, RoutedEventArgs e)
		{
			KeyHandler.textBox = (TextBox)sender;
		}
	}
}
