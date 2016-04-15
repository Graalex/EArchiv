using System.Windows.Controls;
using Mariupolgaz.EArchiv.Abonent.ViewModel;

namespace Mariupolgaz.EArchiv.Abonent.Views
{
	/// <summary>
	/// Interaction logic for AbonentView.xaml
	/// </summary>
	public partial class AbonentView : UserControl
	{
		public AbonentView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="viewModel">Контекст данных для вида</param>
		public AbonentView(AbonentViewModel viewModel)
		:this()
		{
			DataContext = viewModel;
		}
	}
}
