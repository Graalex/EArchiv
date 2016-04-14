using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mariupolgaz.EArchiv.Finder
{
	/// <summary>
	/// Interaction logic for FinderView.xaml
	/// </summary>
	public partial class FinderView : UserControl
	{
		/// <summary>
		/// 
		/// </summary>
		public FinderView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		public FinderView(FinderViewModel viewModel)
		:this()
		{
			DataContext = viewModel;
		}
	}
}
