using System.Windows.Controls;
using Mariupolgaz.EContract.Finder.ViewModels;

namespace Mariupolgaz.EContract.Finder.Views
{
	/// <summary>
	/// Interaction logic for ContractFinderView.xaml
	/// </summary>
	public partial class ContractFinderView : UserControl
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public ContractFinderView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dataContext">Контекст данных</param>
		public ContractFinderView(ContractFinderViewModel dataContext) : this()
		{
			this.DataContext = dataContext;
		}
	}
}
