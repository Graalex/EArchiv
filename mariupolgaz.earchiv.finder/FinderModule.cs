using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Regions;
using Mariupolgaz.EArchiv.Common;

namespace Mariupolgaz.EArchiv.Finder
{
	/// <summary>
	/// Модуль поиска абонентов
	/// </summary>  
	public class FinderModule: IModule
    {
		private readonly IUnityContainer _container;
		private readonly IRegionManager _manager;

		/// <summary>
		/// Создает экземпляр <see cref="FinderModule"/>
		/// </summary>
		/// <param name="container">Контейнер зависимостей</param>
		/// <param name="manager">Менеджер регионов</param>
		/// <exception cref="ArgumentNullException"/>
		public FinderModule(IUnityContainer container, IRegionManager manager)
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
			_container.RegisterType<IFinderService, FinderService>(new ContainerControlledLifetimeManager());
			_manager.RegisterViewWithRegion(RegionNames.FinderRegion, () => _container.Resolve<FinderView> ());
		}
	}
}
