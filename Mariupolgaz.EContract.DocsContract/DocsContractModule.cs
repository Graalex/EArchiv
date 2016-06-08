using System;
using Mariupolgaz.EArchiv.Common;
using Mariupolgaz.EContract.DocsContract.View;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EContract.DocsContract
{
	/// <summary>
	/// Представляет модуль отображения электронных копий договоров
	/// </summary>
	public class DocsContractModule : IModule
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Создает экземпляр <see cref="DocsContractModule"/>
		/// </summary>
		/// <param name="container">Контейнер зависимостей</param>
		public DocsContractModule(IUnityContainer container)
		{
			if (container == null) throw new ArgumentNullException("container");

			_container = container;
		}

		/// <summary>
		/// Инициализирует модуль
		/// </summary>
		public void Initialize()
		{
			_container.RegisterType<object, DocsContractView>(
				ViewNames.DocsContract,
				new ContainerControlledLifetimeManager()
			);
		}
	}
}
