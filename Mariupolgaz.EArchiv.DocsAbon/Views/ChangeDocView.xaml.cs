using System;
using System.Windows;
using Mariupolgaz.EArchiv.DocsAbon.ViewModels;

namespace Mariupolgaz.EArchiv.DocsAbon.Views
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class ChangeDocView : Window
	{
		public ChangeDocView()
		{
			InitializeComponent();
		}

		public ChangeDocView(ChangeDocViewModel viewModel)
			: this()
		{
			DataContext = viewModel;
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
