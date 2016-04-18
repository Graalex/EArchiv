using System.Windows;
using Mariupolgaz.EArchiv.DocsAbon.ViewModels;

namespace Mariupolgaz.EArchiv.DocsAbon.Views
{
	/// <summary>
	/// Interaction logic for DocAddDialog.xaml
	/// </summary>
	public partial class DocAddDialog : Window
	{
		public DocAddDialog()
		{
			InitializeComponent();
		}

		public DocAddDialog(DocAddViewModel viewModel)
			:this()
		{
			DataContext = viewModel;
		}
	}
}
