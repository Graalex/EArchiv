using System;
using System.Windows;
using Mariupolgaz.EContract.DocsContract.ViewModel;

namespace Mariupolgaz.EContract.DocsContract.View
{
	/// <summary>
	/// Interaction logic for ContractDocEditDialog.xaml
	/// </summary>
	public partial class ContractDocEditDialog : Window
	{
		/// <summary>
		/// 
		/// </summary>
		public ContractDocEditDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		public ContractDocEditDialog(ContractDocEditViewModel viewModel) : this()
		{
			this.DataContext = viewModel;
			if (viewModel.Close == null) {
				viewModel.Close = new Action(this._closes);
			}
		}

		private void _closes()
		{
			this.DialogResult = true;
			this.Close();
		}
	}
}
