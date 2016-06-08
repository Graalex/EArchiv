using System.Windows.Controls;
using Mariupolgaz.EContract.DocsContract.ViewModel;

namespace Mariupolgaz.EContract.DocsContract.View
{
	/// <summary>
	/// Interaction logic for DocContractView.xaml
	/// </summary>
	public partial class DocsContractView : UserControl
	{
		/// <summary>
		/// Содает экземпляр <see cref="DocsContractView"/>
		/// </summary>
		public DocsContractView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Содает экземпляр <see cref="DocsContractView"/>
		/// </summary>
		/// <param name="dataContext">Контекст данных</param>
		public DocsContractView(DocsContarctViewModel dataContext) : this()
		{
			DataContext = dataContext;
		}
	}
}
