using System.Windows.Controls;
using Mariupolgaz.EArchiv.Security.ViewModel;

namespace Mariupolgaz.EArchiv.Security.Views
{
	/// <summary>
	/// Interaction logic for LoginNView.xaml
	/// </summary>
	public partial class LoginNView : UserControl
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public LoginNView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dataContext">Контекст данных</param>
		public LoginNView(LoginViewModel dataContext) :this()
		{
			DataContext = dataContext;
		}

	}
}
