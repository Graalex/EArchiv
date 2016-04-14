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
		
		/// <summary>
		/// Создает экземпляр <see cref="FinderModule"/>
		/// </summary>
		/// <param name="container">Контейнер зависимостей</param>
		/// <exception cref="ArgumentNullException"/>
		public FinderModule(IUnityContainer container)
		{
			if (container == null) throw new ArgumentNullException("container");
			
			_container = container;
		}

		/// <summary>
		/// Иницилизация модуля
		/// </summary>
		public void Initialize()
		{
			_container.RegisterType<IFinderService, FinderService>(new ContainerControlledLifetimeManager());
		}
	}
}
