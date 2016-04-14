using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Mariupolgaz.EArchiv
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
