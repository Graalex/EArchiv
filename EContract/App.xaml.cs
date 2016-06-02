using System;
using System.Windows;
using Mariupolgaz.EArchiv.Common;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

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

				// устанавливаем для региона LeftSide вид формы входа в систему
				IRegionManager manager = ServiceLocator.Current.GetInstance<IRegionManager>();
				manager.RequestNavigate(RegionNames.LeftRegion, new Uri(ViewNames.Login, UriKind.Relative));
				
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
