using System;
using Mariupolgaz.EArchiv.Common;
using Mariupolgaz.EArchiv.Common.Servises;
using Mariupolgaz.EArchiv.Document.Services;
using Mariupolgaz.EArchiv.Document.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Mariupolgaz.EArchiv.Document
{
	/// <summary>
	/// Представляет модуль работы с документами
	/// </summary>
	public class DocumentModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _manager;

		/// <summary>
		/// <see cref="DocumentModule"/>
		/// </summary>
		/// <param name="container">Экземпляр реализующий интерфейс контейнера зависимостей</param>
		/// <param name="manager">Экземпляр реализующий интерфейс менеджера регионов</param>
		public DocumentModule(IUnityContainer container, IRegionManager manager)
		{
			if (container == null) throw new ArgumentNullException("container");
			if (manager == null) throw new ArgumentNullException("manager");

			_container = container;
			_manager = manager;
		}

		/// <summary>
		/// Инициализирует модуль работы с документами
		/// </summary>
		public void Initialize()
		{
			_container.RegisterType<IDocumentService, DocumentService>(new ContainerControlledLifetimeManager());
			_manager.RegisterViewWithRegion(RegionNames.KindsListRegion, typeof(KindListView));
		}
	}
}
