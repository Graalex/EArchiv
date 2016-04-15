using System.Windows.Controls;
using Mariupolgaz.EArchiv.DocsAbon.ViewModels;

namespace Mariupolgaz.EArchiv.DocsAbon.Views
{
	/// <summary>
	/// Interaction logic for ClassDocView.xaml
	/// </summary>
	public partial class ClassDocView : UserControl
	{
		/// <summary>
		/// 
		/// </summary>
		public ClassDocView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		public ClassDocView(ClassDocViewModel viewModel)
			:this()
		{
			DataContext = viewModel;
		}
	}
}
