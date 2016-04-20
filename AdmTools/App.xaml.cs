using System;
using System.Windows;

namespace AdmTools
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			try {
				Bootstraper bootstraper = new Bootstraper();
				bootstraper.Run();
			}

			catch (Exception exp) {
				MessageBox.Show(exp.Message, "Ошибка приложения");
			}
		}
	}
}
