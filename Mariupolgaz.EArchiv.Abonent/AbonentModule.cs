using System;
using Mariupolgaz.EArchiv.Abonent.Views;
using Mariupolgaz.EArchiv.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EArchiv.Abonent
{
	/// <summary>
	/// Модуль оработки абонентов.
	/// </summary>
	public class AbonentModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _manager;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="container">Контейнер зависимостей</param>
		/// <param name="manager">Менеджер регионов</param>
		public AbonentModule(IUnityContainer container, IRegionManager manager)
		{
			if (container == null) throw new ArgumentNullException("container");
			if (manager == null) throw new ArgumentNullException("manager");

			_container = container;
			_manager = manager;
		}

		/// <summary>
		/// Иницилизация модуля
		/// </summary>
		public void Initialize()
		{
			_manager.RegisterViewWithRegion(RegionNames.AbonentRegion, () => _container.Resolve<AbonentView>());
		}
	}
}
