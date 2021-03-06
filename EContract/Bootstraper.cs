﻿using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;

namespace EContract
{
	/// <summary>
	/// Загрузчик оболочки
	/// </summary>
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
			Application.Current.MainWindow.Left = SystemParameters.WorkArea.Left;
			Application.Current.MainWindow.Top = SystemParameters.WorkArea.Top;
			Application.Current.MainWindow.Width = SystemParameters.WorkArea.Width;
			Application.Current.MainWindow.Height = SystemParameters.WorkArea.Height;

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
