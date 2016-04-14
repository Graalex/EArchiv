using System.Windows.Controls;
using Mariupolgaz.EArchiv.Document.ViewModels;

namespace Mariupolgaz.EArchiv.Document.Views
{
	/// <summary>
	/// Представляет вид для списка типов документов
	/// </summary>
	public partial class KindListView : UserControl
	{
		/// <summary>
		/// Создает экземпляр <see cref="KindListView"/>
		/// </summary>
		/// <param name="viewModel">Экземпляр модели вида</param>
		public KindListView(KindListViewModel viewModel)
		{
			InitializeComponent();
			this.DataContext = viewModel;
		}
	}
}
