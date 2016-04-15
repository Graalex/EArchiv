using System.Windows.Controls;
using Mariupolgaz.EArchiv.DocsAbon.ViewModels;

namespace Mariupolgaz.EArchiv.DocsAbon.Views
{
	/// <summary>
	/// Interaction logic for KindDocView.xaml
	/// </summary>
	public partial class KindDocView : UserControl
	{
		/// <summary>
		/// 
		/// </summary>
		public KindDocView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		public KindDocView(KindDocViewModel viewModel)
			: this()
		{
			DataContext = viewModel;
		}
	}
}
