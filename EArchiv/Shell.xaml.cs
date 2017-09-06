using System.Windows;

namespace Mariupolgaz.EArchiv
{
	/// <summary>
	/// Interaction logic for Shell.xaml
	/// </summary>
	public partial class Shell : Window
	{
		/// <summary>
		/// 
		/// </summary>
		public Shell()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		public Shell(ShellViewModel viewModel)
			:this()
		{
			DataContext = viewModel;
		}
	}
}
