using System;
using Mariupolgaz.EArchiv.Common;
using Mariupolgaz.EArchiv.Common.Servises;
using Mariupolgaz.EContract.Finder.Services;
using Mariupolgaz.EContract.Finder.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EContract.Finder
{
	/// <summary>
	/// Модуль поиска договоров в базе 1С
	/// </summary>  
	public class ContractFinderModule : IModule
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Создает экземпляр <see cref="ContractFinderModule"/>
		/// </summary>
		/// <param name="container">Экземпляр контейнера зависимостей</param>
		public ContractFinderModule(IUnityContainer container)
		{
			if (container == null) throw new ArgumentNullException("container");

			_container = container;
		}
		
		/// <summary>
		/// Иницилизация модуля
		/// </summary>
		public void Initialize()
		{
			_container.RegisterType<object, ContractFinderView>(
				ViewNames.ContractFinder,
				new ContainerControlledLifetimeManager()
			);
			_container.RegisterType<object, ModernFinderView>(
				ViewNames.ModernFinder,
				new ContainerControlledLifetimeManager()
			);

			_container.RegisterType<IContractFinderService, ContractFinderService>(
				new ContainerControlledLifetimeManager()
			);
		}
	}
}
