using System.Windows.Controls;
using Mariupolgaz.EArchiv.Security.ViewModel;

namespace Mariupolgaz.EArchiv.Security.Views
{
	/// <summary>
	/// Interaction logic for RegisterView.xaml
	/// </summary>
	public partial class RegisterView : UserControl
	{
		public RegisterView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Конструктор с передачей контекста данных
		/// </summary>
		/// <param name="viewModel">Контекст данных</param>
		public RegisterView(RegisteredViewModel viewModel)
			:this()
		{
			DataContext = viewModel;
		}

		private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
		{

		}
	}
}
