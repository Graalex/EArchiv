using System;
using System.Windows;
using Mariupolgaz.EContract.DocsContract.ViewModel;

namespace Mariupolgaz.EContract.DocsContract.Views
{
	/// <summary>
	/// Interaction logic for DocAddDialog.xaml
	/// </summary>
	public partial class ContractDocAddDialog : Window
	{
		/// <summary>
		/// 
		/// </summary>
		public ContractDocAddDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		public ContractDocAddDialog(ContractDocAddViewModel viewModel)
			:this()
		{
			DataContext = viewModel;
			if(viewModel.Close == null) {
				viewModel.Close = new Action(this._closes);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void _closes()
		{
			this.DialogResult = true;
			this.Close();
		}
	}
}
