using System;
using Mariupolgaz.EArchiv.Common.Servises;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EContract.Worker
{
	/// <summary>
	/// 
	/// </summary>
	public class WorkerModule : IModule
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// <see cref="DocumentModule"/>
		/// </summary>
		/// <param name="container">Экземпляр реализующий интерфейс контейнера зависимостей</param>
		public WorkerModule(IUnityContainer container)
		{
			if (container == null) throw new ArgumentNullException("container");

			_container = container;
		}

		/// <summary>
		/// Инициализирует модуль работы с документами
		/// </summary>
		public void Initialize()
		{
			_container.RegisterType<IWorkerService, WorkerService>(new ContainerControlledLifetimeManager());
		}
	}
}

