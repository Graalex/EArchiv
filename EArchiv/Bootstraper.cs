using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Modularity;

namespace Mariupolgaz.EArchiv
{
	public class Bootstraper: UnityBootstrapper
	{
		/// <summary>
		/// Создает главное окно приложения
		/// </summary>
		/// <returns>Объект главное окно</returns>
		protected override System.Windows.DependencyObject CreateShell()
		{
			return ServiceLocator.Current.GetInstance<Shell>();
		}

		/// <summary>
		/// Иницилизация главного окна приложения
		/// </summary>
		protected override void InitializeShell()
		{
			Application.Current.MainWindow = (Window)Shell;
			Application.Current.MainWindow.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width / 8;
			Application.Current.MainWindow.Top = SystemParameters.WorkArea.Top;
			Application.Current.MainWindow.Width = 1200;
			Application.Current.MainWindow.Height = 720;

			//Application.Current.MainWindow.Left = 
			Application.Current.MainWindow.Show();
		}

		/// <summary>
		/// Создает каталог для модулей
		/// </summary>
		/// <returns>Созданный каталог</returns>
		protected override IModuleCatalog CreateModuleCatalog()
		{
			return new ConfigurationModuleCatalog();
		}
	}
}
