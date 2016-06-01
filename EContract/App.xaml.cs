using System;
using System.Windows;

namespace EContract
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Обработка события OnStartup приложения
		/// </summary>
		/// <param name="e"></param>
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			try {
				Bootstraper bootstraper = new Bootstraper();
				bootstraper.Run();
			}

			catch (Exception exp) {
				MessageBox.Show(
					exp.Message,
					"Ошибка приложения",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}
	}
}
