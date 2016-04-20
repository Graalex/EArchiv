using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Mariupolgaz.EArchiv.Common.Servises;
using Mariupolgaz.EArchiv.Security.Service;
using Mariupolgaz.EArchiv.Common;
using Mariupolgaz.EArchiv.Security.Views;

namespace Mariupolgaz.EArchiv.Security
{
	/// <summary>
	/// Реализация сервиса авторизации, аутентификации и аудита.
	/// </summary>
	public class SecurityModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _manager;

		/// <summary>
		/// Создает экземпляр <see cref="SecurityModule"/>.
		/// </summary>
		/// <param name="container">Экземпляр контейнера зависимостей <see cref="IUnityContainer"/></param>
		/// <param name="manager">Экземпляр <see cref="IRegionManager"/></param>
		public SecurityModule(IUnityContainer container, IRegionManager manager)
		{
			if (container == null) throw new ArgumentNullException("container");
			if (manager == null) throw new ArgumentNullException("manager");

			_container = container;
			_manager = manager;
		}

		/// <summary>
		/// Иницилизация модуля.
		/// </summary>
		public void Initialize()
		{
			_container.RegisterType<ISecurityService, SecurityService>(
				new ContainerControlledLifetimeManager()
			);
			_container.RegisterType<IRegisteredService, RegisteredService>(
				new ContainerControlledLifetimeManager()
			);

			_manager.RegisterViewWithRegion(
				RegionNames.LoginRegion,
				() => _container.Resolve<LoginView>()
			);
			_manager.RegisterViewWithRegion(
				RegionNames.RegisterRegion,
				() => _container.Resolve<RegisterView>()
			);
		}
	}
}
