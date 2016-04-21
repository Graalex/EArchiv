using System;
using System.Windows;
using System.Windows.Controls;
using Mariupolgaz.EArchiv.Common.Models;
using Mariupolgaz.EArchiv.Security.ViewModel;

namespace Mariupolgaz.EArchiv.Security.Views
{
	/// <summary>
	/// Иницилизация для LoginView.xaml
	/// </summary>
	public partial class LoginView : UserControl
	{
		/// <summary>
		/// Создает экземпляр <see cref="LoginView"/>
		/// </summary>
		public LoginView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Создает экземпляр <see cref="LoginView"/>
		/// </summary>
		/// <param name="viewModel">Контекст данных для вида</param>
		public LoginView(LoginViewModel viewModel)
			:this()
		{
			DataContext = viewModel;
		}

	}
}
