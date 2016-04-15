using System.Windows.Controls;
using Mariupolgaz.EArchiv.DocsAbon.ViewModels;

namespace Mariupolgaz.EArchiv.DocsAbon.Views
{
	/// <summary>
	/// Interaction logic for DocumentView.xaml
	/// </summary>
	public partial class DocumentView : UserControl
	{
		/// <summary>
		/// 
		/// </summary>
		public DocumentView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		public DocumentView(DocumentViewModel viewModel)
			: this()
		{
			DataContext = viewModel;
		}
	}
}
