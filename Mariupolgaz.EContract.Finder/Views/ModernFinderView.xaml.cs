using System.Windows.Controls;
using Mariupolgaz.EContract.Finder.ViewModels;

namespace Mariupolgaz.EContract.Finder.Views
{
	/// <summary>
	/// Форма поиска и выбора контрагентов и контрактов
	/// </summary>
	public partial class ModernFinderView : UserControl
	{
		/// <summary>
		/// Создает экземпляр <see cref="ModernFinderView"/>
		/// </summary>
		public ModernFinderView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Создает экземпляр <see cref="ModernFinderView"/>, с передаваемым контекстом данных
		/// </summary>
		/// <param name="viewModel">Экземпляр <see cref="ContractFinderViewModel"/> для контекста данных элемента управления.</param>
		public ModernFinderView(ContractFinderViewModel viewModel)
		:base()
		{
			this.DataContext = viewModel;
		}
	}
}
